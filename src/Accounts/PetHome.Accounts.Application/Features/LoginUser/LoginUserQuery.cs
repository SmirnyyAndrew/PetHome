using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.LoginAccount;
public record LoginUserQuery(string Email, string Password) : IQuery;
