using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.EditMessageInDiscussion;
public record EditMessageInDiscussionCommand(
    Guid DiscussionId,
    Guid UserId,
    Guid MessageId,
    string NewMessageText) : ICommand;
