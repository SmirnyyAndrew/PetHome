using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<VolunteerRequest>> SeedVolunteerRequests(int volunteerRequestsCountToSeed)
    {
        List<VolunteerRequest> result = new List<VolunteerRequest>();

        for (int i = 0; i < volunteerRequestsCountToSeed; i++)
        {
            UserId userId = UserId.Create().Value;
            VolunteerInfo volunteerInfo = VolunteerInfo.Create($"Info {i}").Value;
            DiscussionId discussionId = DiscussionId.Create().Value;

            VolunteerRequest request = new VolunteerRequest(userId, volunteerInfo, discussionId);
            result.Add(request);
        }

        await _writeDbContext.AddRangeAsync(result);
        await _writeDbContext.SaveChangesAsync();
        return result;
    }
}
