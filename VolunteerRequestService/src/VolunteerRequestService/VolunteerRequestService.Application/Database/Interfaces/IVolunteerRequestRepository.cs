using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Database.Interfaces;
public interface IVolunteerRequestRepository
{
    public Task Add(VolunteerRequest volunteerRequest);

    public Task Add(IEnumerable<VolunteerRequest> volunteerRequests);

    public Task Remove(VolunteerRequest volunteerRequest);

    public Task Remove(IEnumerable<VolunteerRequest> volunteerRequest);

    public void Update(VolunteerRequest volunteerRequest);

    public void Update(IEnumerable<VolunteerRequest> volunteerRequest);

    public Task<VolunteerRequest?> GetById(Guid volunteerRequestId, CancellationToken ct);

    public Task<IReadOnlyList<VolunteerRequest>> GetByUserId(Guid userId, CancellationToken ct);

    public Task<IReadOnlyList<VolunteerRequest>> GetByAdminId(Guid adminId, CancellationToken ct);

    public Task<IReadOnlyList<VolunteerRequest>> GetByStatus(VolunteerRequestStatus status, CancellationToken ct);
}
