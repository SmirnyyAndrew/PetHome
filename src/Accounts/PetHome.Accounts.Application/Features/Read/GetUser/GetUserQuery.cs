using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Read.GetUser;
public record GetUserQuery(Guid UserId) : IQuery;
