using CSharpFunctionalExtensions;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Contracts.CreateDiscussion;
public interface ICreateDiscussionContract
{
    public Task<Result<DiscussionId, ErrorList>> Execute(
        CreateDiscussionCommand request, CancellationToken ct);

    public Task<Result<DiscussionId, ErrorList>> Execute(
        IEnumerable<UserId> userIds, CancellationToken ct);
}
