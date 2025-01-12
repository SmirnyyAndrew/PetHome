using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Relation;

namespace PetHome.Discussions.Domain;
public class Discussion
{
    public DiscussionId Id { get; private set; }
    public RelationId RelationId { get; private set; }
    public List<User> Users { get; private set; } = [];
    public List<Message> Messages { get; private set; } = [];
    public DiscussionStatus Status { get; private set; }

    private static Error DiscussionCloseError = Errors.Validation("Дискуссия закрыта");
    private static Error UsersCountError = Errors.Validation("В дискуссии должно учавствовать от 2х участников");
    private static Error IsNotParticipantError = Errors.Validation($"User не является участником дискуссии");

    public Discussion(
        RelationId relationId,
        IEnumerable<User> users)
    {
        Id = DiscussionId.Create().Value;
        RelationId = relationId;
        Users = users.ToList();
        Status = DiscussionStatus.Open;
    }


    public static Result<Discussion, Error> Create(
        RelationId relationId, IEnumerable<User> users)
    {
        if (users.Count() < 2)
            return UsersCountError;

        return new Discussion(relationId, users);
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        bool isDiscussionParticipant = Users.Select(u => u.Id)
            .Contains(message.UserId);

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

    public UnitResult<Error> AddUser(User user)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        Users.Add(user);
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddUser(IEnumerable<User> users)
    {
        if (Status == DiscussionStatus.Close)
            return DiscussionCloseError;

        Users.AddRange(users);
        return Result.Success<Error>();
    }

    public void Close() => Status = DiscussionStatus.Close;
    public void ReOpen() => Status = DiscussionStatus.Open;
}
