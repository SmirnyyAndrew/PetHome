using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Read.GetUser;
public record GetUserQuery(Guid UserId) : IQuery;
