using CSharpFunctionalExtensions;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Contracts;
public interface ICreateDiscussionContract
{
    public Task<Result<DiscussionId, ErrorList>> Execute(
        RelationId relationId, IEnumerable<UserId> userIds, CancellationToken ct);

    public Task<Result<DiscussionId, ErrorList>> Execute(
        IEnumerable<UserId> userIds, CancellationToken ct);
}
