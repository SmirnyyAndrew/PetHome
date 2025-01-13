using Microsoft.EntityFrameworkCore;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Discussions.Domain;

namespace PetHome.Discussions.Application.Database.Interfaces;
public interface IDiscussionRepository
{
    public Task AddDiscussion(Discussion discussion);

    public Task AddDiscussion(IEnumerable<Discussion> discussions);

    public Task RemoveDiscussion(Discussion discussion);

    public Task RemoveDiscussion(IEnumerable<Discussion> discussions);

    public Task<Discussion?> GetDiscussionById(Guid discussionId, CancellationToken ct);

    public Task<IReadOnlyList<Discussion>> GetDiscussionByRelationId(Guid relationId, CancellationToken ct);

    public Task<IReadOnlyList<Discussion>> GetDiscussionByStatus(DiscussionStatus status, CancellationToken ct);

     

    public Task AddRelation(Relation relation);

    public Task AddRelation(IEnumerable<Relation> relations);

    public Task RemoveRelation(Relation relation);

    public Task RemoveRelation(IEnumerable<Relation> relations);

    public Task<Relation?> GetRelationById(Guid relationId, CancellationToken ct);

    public Task<Relation?> GetRelationByName(RelationName relationName, CancellationToken ct);
}
