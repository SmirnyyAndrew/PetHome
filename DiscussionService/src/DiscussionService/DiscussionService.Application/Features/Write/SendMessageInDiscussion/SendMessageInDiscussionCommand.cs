using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Write.SendMessageInDiscussion;
public record SendMessageInDiscussionCommand(Guid DiscussionId, Guid UserId, string Message) : ICommand;
