using PetHome.Volunteers.Contracts.Dto;

namespace PetHome.Volunteers.Contracts.Contracts;
public interface IGetPetInformationContract
{
    public Task<PetDto?> Execute(Guid VolunteerId, Guid PetId, CancellationToken ct);
}
