using CSharpFunctionalExtensions;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Contracts;
public interface ICreateDiscussionContract
{
    public Task<Result<DiscussionId, ErrorList>> CreateDiscussion(
        RelationId relationId, IEnumerable<UserId> userIds, CancellationToken ct);

    public Task<Result<DiscussionId, ErrorList>> CreateDiscussion(
        IEnumerable<UserId> userIds, CancellationToken ct);
}
