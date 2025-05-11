using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Write.EditMessageInDiscussion;
public record EditMessageInDiscussionCommand(
    Guid DiscussionId,
    Guid UserId,
    Guid MessageId,
    string NewMessageText) : ICommand;
