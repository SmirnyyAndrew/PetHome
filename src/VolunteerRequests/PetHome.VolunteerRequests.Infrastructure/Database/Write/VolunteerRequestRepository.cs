using Microsoft.EntityFrameworkCore;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Infrastructure.Database.Write;
public class VolunteerRequestRepository(VolunteerRequestDbContext dbContext)
    : IVolunteerRequestRepository
{
    public async Task Add(VolunteerRequest volunteerRequest)
    {
        await dbContext.VolunteerRequests.AddAsync(volunteerRequest);
    }

    public async Task Add(IEnumerable<VolunteerRequest> volunteerRequests)
    {
        await dbContext.VolunteerRequests.AddRangeAsync(volunteerRequests);
    }

    public async Task Remove(VolunteerRequest volunteerRequest)
    {
        dbContext.VolunteerRequests.Remove(volunteerRequest);
    }

    public async Task Remove(IEnumerable<VolunteerRequest> volunteerRequest)
    {
        dbContext.VolunteerRequests.RemoveRange(volunteerRequest);
    }

    public async Task<IReadOnlyList<VolunteerRequest>> GetByUserId(Guid userId, CancellationToken ct)
    {
        var userVolunteerRequests = await dbContext.VolunteerRequests
            .Where(v => v.UserId == userId)
            .ToListAsync(ct);
        return userVolunteerRequests;
    }

    public async Task<IReadOnlyList<VolunteerRequest>> GetByAdminId(Guid adminId, CancellationToken ct)
    {
        var adminVolunteerRequests = await dbContext.VolunteerRequests
            .Where(v => v.AdminId == adminId)
            .ToListAsync(ct);
        return adminVolunteerRequests;
    }

    public async Task<IReadOnlyList<VolunteerRequest>> GetByStatus(VolunteerRequestStatus status, CancellationToken ct)
    {
        var adminVolunteerRequests = await dbContext.VolunteerRequests
            .Where(v => v.Status == status)
            .ToListAsync(ct);
        return adminVolunteerRequests;
    }



}
