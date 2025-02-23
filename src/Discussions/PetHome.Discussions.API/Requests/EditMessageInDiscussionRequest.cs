using PetHome.Discussions.Application.Features.Write.EditMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record EditMessageInDiscussionRequest(
    Guid UserId,
    Guid MessageId,
    string NewMessageText)
{
    public EditMessageInDiscussionCommand ToCommand(Guid discussionId) 
        => new EditMessageInDiscussionCommand(
            discussionId,
            UserId,
            MessageId,
            NewMessageText);
}
