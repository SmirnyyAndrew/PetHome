using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Write.CreateDiscussionUsingContract;
public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> UsersIds) : ICommand;
