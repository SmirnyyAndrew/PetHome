using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.OpenDiscussion;
public record OpenDiscussionCommand(Guid DiscussionId) : ICommand;
