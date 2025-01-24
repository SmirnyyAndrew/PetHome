using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.SendMessageInDiscussion;
public record SendMessageInDiscussionCommand(Guid DiscussionId, Guid UserId, string Message) : ICommand;
