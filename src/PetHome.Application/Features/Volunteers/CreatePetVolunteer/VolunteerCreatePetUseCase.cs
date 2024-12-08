using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.CreatePetVolunteer;
public class VolunteerCreatePetUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<VolunteerCreatePetUseCase> _logger;

    public VolunteerCreatePetUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<VolunteerCreatePetUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }


    public async Task<Result<Pet, Error>> Execute(VolunteerCreatePetRequest petRequest, CancellationToken ct)
    {
        var mainInfoDto = petRequest.MainInfoDto;

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
        RequisitesDetails requisitesDetails = RequisitesDetails.Create(requisites).Value;


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
             requisitesDetails);

        if (result.IsFailure)
        {
            _logger.LogError($"Создание pet через контроллер volunteer завершился с ошибкой {result.Error}");
            return result.Error;
        }

        Pet pet = result.Value;
        _logger.LogInformation($"Pet с id {pet.Id} и volunteer_id {pet.VolunteerId} создан");
        return pet;
    }
}
