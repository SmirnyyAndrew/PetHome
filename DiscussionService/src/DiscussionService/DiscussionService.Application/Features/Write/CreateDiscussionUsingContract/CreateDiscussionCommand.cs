using PetHome.Core.Interfaces.FeatureManagment;

namespace DiscussionService.Application.Features.Write.CreateDiscussionUsingContract;
public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> UsersIds) : ICommand;
