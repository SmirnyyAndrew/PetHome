using PetManagementService.Application.Database;
using PetManagementService.Contracts.Contracts.Pet;
using PetManagementService.Contracts.Dto;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Contracts.PetEntity;
public class GetPetInformationUsingContract(IVolunteerRepository repository)
    : IGetPetInformationContract
{
    public async Task<PetDto?> Execute(Guid VolunteerId, Guid PetId, CancellationToken ct)
    {
        var getVolunteerResult = await repository.GetById(VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return null;

        Volunteer volunteer = getVolunteerResult.Value;
        Pet? pet = volunteer.Pets.FirstOrDefault(p => p.Id == PetId);
        if (pet is null)
            return null;

        PetDto petDto = new PetDto(
           pet.Id,
           pet.Name,
           pet.SpeciesId,
           pet.Description,
           pet.BreedId,
           pet.Color,
           pet.ShelterId,
           pet.Weight,
           pet.IsCastrated,
           pet.BirthDate,
           pet.IsVaccinated,
           pet.Status.ToString(),
           pet.VolunteerId,
           pet.SerialNumber);

        return petDto;
    }
}
