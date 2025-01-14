using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.RemoveMessageInDiscussion;
public record RemoveMessageInDiscussionCommand(Guid DiscussionId, Guid UserId, Guid MessageId) : ICommand; 
