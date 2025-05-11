using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Write.CloseDiscussion;
public record CloseDiscussionCommand(Guid DiscussionId) : ICommand;
