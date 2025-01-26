using PetHome.Accounts.Contracts.User;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.User;
using PetHome.Discussions.Domain;
namespace PetHome.Discussions.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Discussion>> SeedDiscussions(int discussionsCountToSeed = 5)
    {
        List<Discussion> result = new List<Discussion>();

        for (int i = 0; i < discussionsCountToSeed; i++)
        {
            RelationId relationId = RelationId.Create().Value;
            List<UserId> users = new List<UserId>()
            { 
                //TODO:
                 UserId.Create(Guid.NewGuid()).Value,
                 UserId.Create(Guid.NewGuid()).Value
            };

            Discussion discussion = Discussion.Create(relationId, users).Value;

            result.Add(discussion);
        }

        await _dbContext.AddRangeAsync(result);
        await _dbContext.SaveChangesAsync();
        return result;
    }
}
