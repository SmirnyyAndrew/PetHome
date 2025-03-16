namespace PetHome.Discussions.Contracts.Messaging;
public record EditedMessageInDiscussionEvent(
    Guid DiscussionId,
    Guid UserId,
    Guid MessageId,
    string OldMessageText,
    string NewMessageText);