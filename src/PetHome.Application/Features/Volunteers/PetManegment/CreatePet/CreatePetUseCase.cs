using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreatePetVolunteer;
public class CreatePetUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreatePetUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetUseCase(
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        ILogger<CreatePetUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Pet, Error>> Execute(CreatePetCommand petRequest, CancellationToken ct)
    {
        PetMainInfoDto mainInfoDto = petRequest.PetMainInfoDto;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var IsSpeciesExist = await _speciesRepository.GetById(mainInfoDto.SpeciesId, ct);
            if (IsSpeciesExist.IsFailure)
                return Errors.NotFound($"Species с id {mainInfoDto.SpeciesId} не найден");

            var IsBreedExist = IsSpeciesExist.Value.Breeds
                .Any(x => x.Id == mainInfoDto.BreedId);
            if (IsBreedExist == false)
                return Errors.NotFound($"Breed с id {mainInfoDto.SpeciesId} не найден");


            Volunteer volunteer = _volunteerRepository.GetById(petRequest.VolunteerId, ct).Result.Value;
            PetName petName = PetName.Create(mainInfoDto.Name).Value;
            SpeciesId petSpeciesId = SpeciesId.Create(mainInfoDto.SpeciesId).Value;
            Description petDescription = Description.Create(mainInfoDto.Description).Value;
            BreedId petBreedId = BreedId.Create(mainInfoDto.BreedId).Value;
            Color petColor = Color.Create(mainInfoDto.Color).Value;
            PetShelterId shelterId = PetShelterId.Create(mainInfoDto.ShelterId).Value;
            Date petBirthDate = Date.Create(mainInfoDto.BirthDate).Value;
            VolunteerId volunteerId = VolunteerId.Create(petRequest.VolunteerId).Value;

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
                _logger.LogError("Создание pet через контроллер volunteer завершился с ошибкой {0}", result.Error);
                return result.Error;
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
            _logger.LogInformation("Не удалось создать питомца");
            return Errors.Failure("Database.is.failed");
        }
    }
}
