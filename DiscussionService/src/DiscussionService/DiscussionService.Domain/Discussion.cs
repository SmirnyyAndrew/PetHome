using CSharpFunctionalExtensions;
using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.Discussion;
using PetHome.SharedKernel.ValueObjects.Discussion.Message;
using PetHome.SharedKernel.ValueObjects.Discussion.Relation;
using PetHome.SharedKernel.ValueObjects.User;

namespace PetHome.Discussions.Domain;
public class Discussion : DomainEntity<DiscussionId>
{
    public DiscussionId Id { get; private set; }
    public RelationId RelationId { get; private set; }
    public Relation Relation { get; private set; }
    public List<UserId> UserIds { get; private set; } = [];
    public List<Message> Messages { get; private set; } = [];
    public DiscussionStatus Status { get; private set; }

    private static Error DiscussionCloseError = Errors.Validation("Дискуссия закрыта");
    private static Error UsersCountError = Errors.Validation("В дискуссии должно учавствовать от 2х участников");
    private static Error IsNotParticipantError = Errors.Validation($"User не является участником дискуссии");

    private Discussion(DiscussionId id) : base(id) { Id = id; }
    private Discussion(DiscussionId id, RelationId relationId, IEnumerable<UserId> userIds)
        : base(id)
    {
        Id = id;
        RelationId = relationId;
        UserIds = userIds.ToList();
        Status = DiscussionStatus.Open;
    }


    public static Result<Discussion, Error> Create(
        RelationId relationId, IEnumerable<UserId> userIds)
    {
        if (userIds.Count() < 2)
            return UsersCountError;

        Discussion discussion = new Discussion(
            DiscussionId.Create().Value,
            relationId,
            userIds);
        return discussion;
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        bool isDiscussionParticipant = UserIds.Contains(message.UserId);

        if (isDiscussionParticipant is not true)
            return IsNotParticipantError;

        Messages.Add(message);
        return Result.Success<Error>();
    }

    public UnitResult<Error> RemoveMessage(Message message)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        Messages.Remove(message);
        return Result.Success<Error>();
    }

    public UnitResult<Error> EditMessage(MessageId messageId, MessageText newMessageText)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        Message? message = Messages.FirstOrDefault(m => m.Id == messageId);
        if (message is null)
            return Errors.NotFound(nameof(Message));

        message = message.EditMessage(newMessageText);

        return Result.Success<Error>();
    }

    public UnitResult<Error> AddUser(UserId userId)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        UserIds.Add(userId);
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddUser(IEnumerable<UserId> userIds)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        UserIds.AddRange(userIds);
        return Result.Success<Error>();
    }

    public void Close() => Status = DiscussionStatus.Close;
    public void ReOpen() => Status = DiscussionStatus.Open;
}
