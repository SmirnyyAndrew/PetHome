using DiscussionService.Application.Features.Write.EditMessageInDiscussion;

namespace DiscussionService.API.Requests;
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
