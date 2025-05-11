using PetHome.VolunteerRequests.Contracts.Dto;

namespace PetHome.VolunteerRequests.Contracts.Contracts;
public interface IGetVolunteerRequestContract
{
    public Task<VolunteerRequestDto?> Execute(Guid volunteerRequestId, CancellationToken ct);
}
