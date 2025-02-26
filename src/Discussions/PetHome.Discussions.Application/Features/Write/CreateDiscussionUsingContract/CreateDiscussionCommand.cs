using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Discussions.Application.Features.Write.CreateDiscussionUsingContract;
public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> UsersIds) : ICommand;
