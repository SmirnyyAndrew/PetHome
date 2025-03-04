using Microsoft.EntityFrameworkCore;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Domain;

namespace PetHome.Discussions.Infrastructure.Database.Write.Repositories;
public class DiscussionRepository(DiscussionDbContext dbContext) : IDiscussionRepository
{
    public async Task AddDiscussion(Discussion discussion)
    {
        await dbContext.Discussions.AddAsync(discussion);
    }

    public async Task AddDiscussion(IEnumerable<Discussion> discussions)
    {
        await dbContext.Discussions.AddRangeAsync(discussions);
    }

    public async Task RemoveDiscussion(Discussion discussion)
    {
        dbContext.Discussions.Remove(discussion);
    }

    public async Task RemoveDiscussion(IEnumerable<Discussion> discussions)
    {
        dbContext.Discussions.RemoveRange(discussions);
    }

    public async Task UpdateDiscussion(Discussion discussion)
    {
        dbContext.Discussions.Update(discussion);
    }

    public async Task UpdateDiscussion(IEnumerable<Discussion> discussions)
    {
        dbContext.Discussions.UpdateRange(discussions);
    }

    public async Task<Discussion?> GetDiscussionById(Guid discussionId, CancellationToken ct)
    {
        var discussion = await dbContext.Discussions
            .Include(d => d.Relation)
            .FirstOrDefaultAsync(d => d.Id == discussionId);
        return discussion;
    }

    public async Task<IReadOnlyList<Discussion>> GetDiscussionByRelationId(Guid relationId, CancellationToken ct)
    {
        var discussions = await dbContext.Discussions
            .Include(d => d.Relation)
            .Where(d => d.RelationId == relationId)
            .ToListAsync(ct);
        return discussions;
    }

    public async Task<IReadOnlyList<Discussion>> GetDiscussionByStatus(DiscussionStatus status, CancellationToken ct)
    {
        var discussions = await dbContext.Discussions
            .Where(v => v.Status == status)
            .ToListAsync(ct);
        return discussions;
    }




    public async Task AddRelation(Relation relation)
    {
        await dbContext.Relations.AddAsync(relation);
    }

    public async Task AddRelation(IEnumerable<Relation> relations)
    {
        await dbContext.Relations.AddRangeAsync(relations);
    }

    public async Task RemoveRelation(Relation relation)
    {
        dbContext.Relations.Remove(relation);
    }

    public async Task RemoveRelation(IEnumerable<Relation> relations)
    {
        dbContext.Relations.RemoveRange(relations);
    }

    public async Task UpdateRelation(Relation relation)
    {
        dbContext.Relations.Update(relation);
    }

    public async Task UpdateRelation(IEnumerable<Relation> relations)
    {
        dbContext.Relations.UpdateRange(relations);
    }

    public async Task<Relation?> GetRelationById(Guid relationId, CancellationToken ct)
    {
        var discussions = await dbContext.Relations
            .FirstOrDefaultAsync(r => r.Id == relationId);
        return discussions;
    }

    public async Task<Relation?> GetRelationByName(string relationName, CancellationToken ct)
    {
        var discussions = await dbContext.Relations
            .FirstOrDefaultAsync(r => r.Name == relationName);
        return discussions;
    } 
}
