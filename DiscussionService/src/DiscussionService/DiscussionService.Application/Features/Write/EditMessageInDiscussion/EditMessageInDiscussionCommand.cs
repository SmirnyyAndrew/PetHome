using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.EditMessageInDiscussion;
public record EditMessageInDiscussionCommand(
    Guid DiscussionId,
    Guid UserId,
    Guid MessageId,
    string NewMessageText) : ICommand;
