using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.OpenDiscussion;
public record OpenDiscussionCommand(Guid DiscussionId) : ICommand;
