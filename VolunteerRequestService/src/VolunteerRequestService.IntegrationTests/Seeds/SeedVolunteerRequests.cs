using PetHome.SharedKernel.ValueObjects.User;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Domain;

namespace VolunteerRequestService.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<VolunteerRequest>> SeedVolunteerRequests(int volunteerRequestsCountToSeed)
    {
        List<VolunteerRequest> result = new List<VolunteerRequest>();

        for (int i = 0; i < volunteerRequestsCountToSeed; i++)
        {
            UserId userId = UserId.Create().Value;
            VolunteerInfo volunteerInfo = VolunteerInfo.Create($"Info {i}").Value;

            VolunteerRequest request = new VolunteerRequest(
                VolunteerRequestId.Create().Value,
                userId,
                volunteerInfo);
            result.Add(request);
        }

        await _writeDbContext.AddRangeAsync(result);
        await _writeDbContext.SaveChangesAsync();
        return result;
    }
}
