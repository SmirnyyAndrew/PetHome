using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.CloseDiscussion;
public record CloseDiscussionCommand(Guid DiscussionId) : ICommand;
