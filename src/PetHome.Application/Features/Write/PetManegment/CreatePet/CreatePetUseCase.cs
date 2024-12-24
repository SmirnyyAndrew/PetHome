using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.CreatePet;
public class CreatePetUseCase
    : ICommandHandler<Pet, CreatePetCommand>
{
    private readonly IReadDBContext _readDBContext;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreatePetUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePetCommand> _validator;

    public CreatePetUseCase(
        IReadDBContext readDBContext,
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        ILogger<CreatePetUseCase> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreatePetCommand> validator)
    {
        _readDBContext = readDBContext;
        _volunteerRepository = volunteerRepository;
        _speciesRepository = speciesRepository;
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
            return (ErrorList)validationResult.Errors;

        PetMainInfoDto mainInfoDto = createPetCommand.PetMainInfoDto;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var isExistVolunteer = _readDBContext.Volunteers
               .Any(s => s.Id == createPetCommand.VolunteerId);
            if (isExistVolunteer == false)
            {
                _logger.LogError("Волонтёр с id = {0} не найден", mainInfoDto.SpeciesId);
                return (ErrorList)Errors.NotFound($"Волонтёр с id = {mainInfoDto.SpeciesId}");
            }

            var speciesResult = _readDBContext.Species
               .Where(s => s.Id == mainInfoDto.SpeciesId);
            if (speciesResult.Count() == 0)
            {
                _logger.LogError("Вид питомца с id = {0} не найден", mainInfoDto.SpeciesId);
                return (ErrorList)Errors.NotFound($"Вид питомца с id = {mainInfoDto.SpeciesId}");
            }

            var isExistBreed = speciesResult
                .SelectMany(b => b.Breeds)
                .Any(x => x.Id == mainInfoDto.BreedId);
            if (isExistBreed == false)
            {
                _logger.LogError("Порода с id = {0} не найдена", mainInfoDto.SpeciesId);
                return (ErrorList)Errors.NotFound($"Порода с id = {mainInfoDto.SpeciesId}");
            }

            Volunteer volunteer = _volunteerRepository.GetById(createPetCommand.VolunteerId, ct).Result.Value;
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
                return (ErrorList)result.Error;
            }

            Pet pet = result.Value;
            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Pet с id = {0} и volunteer_id = {1} создан", pet.Id.Value, pet.VolunteerId.Value);
            return pet;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogError("Не удалось создать питомца");
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
