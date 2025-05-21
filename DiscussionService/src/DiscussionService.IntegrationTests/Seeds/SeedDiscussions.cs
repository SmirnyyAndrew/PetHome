using PetHome.Discussions.Domain;
using PetHome.SharedKernel.ValueObjects.Discussion.Relation;
using PetHome.SharedKernel.ValueObjects.User;
namespace PetHome.Discussions.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Discussion>> SeedDiscussions(int discussionsCountToSeed)
    {
        List<Discussion> discussions = new List<Discussion>(discussionsCountToSeed);
        RelationName relationName = RelationName.Create(Guid.NewGuid().ToString()).Value;
        Relation relation = Relation.Create(relationName);

        await _dbContext.Relations.AddAsync(relation);
        await _dbContext.SaveChangesAsync();

        for (int i = 0; i < discussionsCountToSeed; i++)
        {
            List<UserId> users = new List<UserId>()
            { 
                //TODO:
                 UserId.Create(Guid.NewGuid()).Value,
                 UserId.Create(Guid.NewGuid()).Value
            };

            Discussion discussion = Discussion.Create(relation.Id, []).Value;

            discussions.Add(discussion);
        }

        await _dbContext.Discussions.AddRangeAsync(discussions);
        await _dbContext.SaveChangesAsync();
        return discussions;
    }
}
