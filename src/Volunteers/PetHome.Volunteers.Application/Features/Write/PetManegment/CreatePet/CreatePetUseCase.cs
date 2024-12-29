using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Framework.Database;
using PetHome.Species.Application.Database;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using IVolunteerReadDbContext = PetHome.Volunteers.Application.Database.IVolunteerReadDbContext;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;
public class CreatePetUseCase
    : ICommandHandler<Pet, CreatePetCommand>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreatePetUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePetCommand> _validator;

    public CreatePetUseCase(
        IVolunteerReadDbContext readDBContext,
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        ILogger<CreatePetUseCase> logger,
        [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
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
            return validationResult.Errors.ToErrorList();

        PetMainInfoDto mainInfoDto = createPetCommand.PetMainInfoDto;

         var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var isExistVolunteer = _readDBContext.Volunteers
               .Any(s => s.Id == createPetCommand.VolunteerId);
            if (isExistVolunteer == false)
            {
                _logger.LogError("Волонтёр с id = {0} не найден", mainInfoDto.SpeciesId);
                return Errors.NotFound($"Волонтёр с id = {mainInfoDto.SpeciesId}").ToErrorList();
            }

            var speciesResult = _readDBContext.Species
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
                return result.Error.ToErrorList();
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
            //transaction.Rollback();
            _logger.LogError("Не удалось создать питомца");
            return Errors.Failure("Database.is.failed").ToErrorList();
        }
    }
}
