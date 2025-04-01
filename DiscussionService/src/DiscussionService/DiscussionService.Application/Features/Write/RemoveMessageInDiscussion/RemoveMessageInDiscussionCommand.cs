using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.RemoveMessageInDiscussion;
public record RemoveMessageInDiscussionCommand(Guid DiscussionId, Guid UserId, Guid MessageId) : ICommand; 
