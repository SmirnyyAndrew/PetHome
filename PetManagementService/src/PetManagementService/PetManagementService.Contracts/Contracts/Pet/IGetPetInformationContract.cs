using PetManagementService.Contracts.Dto;

namespace PetManagementService.Contracts.Contracts.Pet;
public interface IGetPetInformationContract
{
    public Task<PetDto?> Execute(Guid VolunteerId, Guid PetId, CancellationToken ct);
}
