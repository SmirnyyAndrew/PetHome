using PetHome.Volunteers.Contracts.Dto;

namespace PetHome.Volunteers.Contracts.Contracts;
public interface IGetVolunteerInformationContract
{
    public Task<VolunteerDto?> Execute(Guid Id, CancellationToken ct); 
}
