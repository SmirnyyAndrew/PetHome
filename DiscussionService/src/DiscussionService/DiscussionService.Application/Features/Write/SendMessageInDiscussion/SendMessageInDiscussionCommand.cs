using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.SendMessageInDiscussion;
public record SendMessageInDiscussionCommand(Guid DiscussionId, Guid UserId, string Message) : ICommand;
