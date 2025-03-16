namespace PetHome.Discussions.Contracts.Messaging;
public record RemovedMessageInDiscussionEvent(
    Guid DiscussionId, 
    Guid UserId, 
    Guid MessageId,
    string Message);