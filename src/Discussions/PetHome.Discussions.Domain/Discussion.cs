using CSharpFunctionalExtensions;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Message;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Domain;
public class Discussion
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
    private Discussion() { }
    public Discussion(RelationId relationId, IEnumerable<UserId> userIds)
    {
        Id = DiscussionId.Create().Value;
        RelationId = relationId;
        UserIds = userIds.ToList();
        Status = DiscussionStatus.Open;
    }


    public static Result<Discussion, Error> Create(
        RelationId relationId, IEnumerable<UserId> userIds)
    {
        if (userIds.Count() < 2)
            return UsersCountError;

        return new Discussion(relationId, userIds);
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
