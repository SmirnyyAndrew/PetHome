using PetManagementService.Contracts.Dto;

namespace PetManagementService.Contracts.Contracts.VolunteerEntity;
public interface IGetVolunteerInformationContract
{
    public Task<VolunteerDto?> Execute(Guid Id, CancellationToken ct); 
}
