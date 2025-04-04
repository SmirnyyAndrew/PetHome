using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Application.Database;
using PetManagementService.Application.Dto.Pet;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetManagementService.Domain.SpeciesManagment.BreedEntity;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.CreatePet;
public class CreatePetUseCase
    : ICommandHandler<Pet, CreatePetCommand>
{
    private readonly IPetManagementReadDbContext _volunteerReadDBContext;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreatePetUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePetCommand> _validator;

    public CreatePetUseCase(
        IPetManagementReadDbContext volunteerReadDBContext,
        IVolunteerRepository volunteerRepository,
        ILogger<CreatePetUseCase> logger,
        IUnitOfWork unitOfWork, 
        IValidator<CreatePetCommand> validator)
    {
        _volunteerReadDBContext = volunteerReadDBContext;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Pet, ErrorList>> Execute(
        CreatePetCommand createPetCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(createPetCommand, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        PetMainInfoDto mainInfoDto = createPetCommand.PetMainInfoDto;

        var transaction = await _unitOfWork.BeginTransaction(ct);

        var isExistVolunteer = _volunteerReadDBContext.Volunteers
           .Any(s => s.Id == createPetCommand.VolunteerId);
        if (isExistVolunteer == false)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", mainInfoDto.SpeciesId);
            return Errors.NotFound($"Волонтёр с id = {mainInfoDto.SpeciesId}").ToErrorList();
        }

        var speciesResult = _volunteerReadDBContext.Species
           .Where(s => s.Id == mainInfoDto.SpeciesId);
        if (speciesResult.Count() == 0)
        {
            _logger.LogError("Вид питомца с id = {0} не найден", mainInfoDto.SpeciesId);
            return Errors.NotFound($"Вид питомца с id = {mainInfoDto.SpeciesId}").ToErrorList();
        }

        var isExistBreed = speciesResult
            .SelectMany(b => b.Breeds)
            .Any(x => x.Id == mainInfoDto.BreedId);
        if (isExistBreed == false)
        {
            _logger.LogError("Порода с id = {0} не найдена", mainInfoDto.SpeciesId);
            return Errors.NotFound($"Порода с id = {mainInfoDto.SpeciesId}").ToErrorList();
        }

        var getVolunteerResult = await _volunteerRepository.GetById(createPetCommand.VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value;
        PetName petName = PetName.Create(mainInfoDto.Name).Value;
        SpeciesId petSpeciesId = SpeciesId.Create(mainInfoDto.SpeciesId).Value;
        Description petDescription = Description.Create(mainInfoDto.Description).Value;
        BreedId petBreedId = BreedId.Create(mainInfoDto.BreedId).Value;
        Color petColor = Color.Create(mainInfoDto.Color).Value;
        PetShelterId shelterId = PetShelterId.Create(mainInfoDto.ShelterId).Value;
        Date petBirthDate = Date.Create(mainInfoDto.BirthDate).Value;
        VolunteerId volunteerId = VolunteerId.Create(createPetCommand.VolunteerId).Value;

        List<Requisites> requisites = mainInfoDto.Requisites
            .Select(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod).Value)
            .ToList();

        var result = volunteer.CreatePet(
             petName,
             petSpeciesId,
             petDescription,
             petBreedId,
             petColor,
             shelterId,
             mainInfoDto.Weight,
             mainInfoDto.IsCastrated,
             petBirthDate,
             mainInfoDto.IsVaccinated,
             mainInfoDto.Status,
             requisites);

        if (result.IsFailure)
        {
            _logger.LogError("Создание питомца через контроллер volunteer завершился с ошибкой {0}", result.Error);
            return result.Error.ToErrorList();
        }

        Pet pet = result.Value;
        await _volunteerRepository.Update(volunteer, ct);

        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        _logger.LogInformation("Pet с id = {0} и volunteer_id = {1} создан", pet.Id.Value, pet.VolunteerId.Value);
        return pet;
    }
}
