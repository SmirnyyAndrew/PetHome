using PetHome.Discussions.Domain;
using PetHome.SharedKernel.ValueObjects.Discussion.Message;
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
            List<UserId> usersIds = new List<UserId>()
            {  
                 UserId.Create(Guid.NewGuid()).Value,
                 UserId.Create(Guid.NewGuid()).Value
            };
            MessageText messageText = MessageText.Create(Guid.NewGuid().ToString()).Value; 
            Message message = Message.Create(messageText, usersIds[0]);

            Discussion discussion = Discussion.Create(relation.Id, usersIds).Value;
            discussion.AddMessage(message);
             
            discussions.Add(discussion);
        }

        await _dbContext.Discussions.AddRangeAsync(discussions);
        await _dbContext.SaveChangesAsync();

         
        return discussions;
    }
}
