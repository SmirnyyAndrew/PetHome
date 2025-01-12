using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Message;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Domain;
public class Message
{
    public MessageId Id { get; private set; }
    public MessageText? Text { get; private set; }
    public UserId UserId { get; private set; }
    public User User { get; private set; }
    public DiscussionId DiscussionId { get; private set; }
    public Discussion Discussion { get; private set; }
    public Date CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;

    private Message() { }

    private Message(MessageText text, UserId userId)
    {
        Id = MessageId.Create().Value;
        CreatedAt = Date.Create().Value;
        Text = text; 
        //UserId = userId;
    }

    public static Message Create(MessageText text, UserId userId) => new Message(text, userId);

    public void EditMessage(MessageText? newText)
    {
        Text = newText;
        IsEdited = true;
    }
}
