using PetHome.Core.Models;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Message;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Discussions.Domain;
public class Message : DomainEntity<MessageId>
{
    public MessageId Id { get; private set; }
    public MessageText? Text { get; private set; }
    public UserId UserId { get; private set; }
    public DiscussionId DiscussionId { get; private set; }
    public Discussion Discussion { get; private set; }
    public Date CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;

    private Message(MessageId id) : base(id) { }

    private Message(MessageId id, MessageText text, UserId userId)
        : base(id)
    {
        Id = id;
        Text = text;
        UserId = userId;
    }

    public static Message Create(MessageText text, UserId userId)
    {
        MessageId id = MessageId.Create().Value;
        Message message = new Message(id, text, userId)
        {
            CreatedAt = Date.Create().Value
        }; 
        return message;
    }

    public Message EditMessage(MessageText newText)
    {
        return new Message(Id, newText, UserId)
        { 
            CreatedAt = CreatedAt,
            IsEdited = true
        };
    }
}
