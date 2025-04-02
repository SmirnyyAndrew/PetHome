using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Write.OpenDiscussion;
public record OpenDiscussionCommand(Guid DiscussionId) : ICommand;
