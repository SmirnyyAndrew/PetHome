using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Read.GetUser;
public record GetUserQuery(Guid UserId) : IQuery;
