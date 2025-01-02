using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.LoginAccount;
public record LoginAccountQuery(string Email, string Password) : IQuery;
