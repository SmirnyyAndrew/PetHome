using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.CloseDiscussion;
public record CloseDiscussionCommand(Guid DiscussionId) : ICommand;
