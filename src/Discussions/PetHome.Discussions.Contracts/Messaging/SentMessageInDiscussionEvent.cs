namespace PetHome.Discussions.Contracts.Messaging;
public record SentMessageInDiscussionEvent(Guid DiscussionId, Guid UserId, string Message);