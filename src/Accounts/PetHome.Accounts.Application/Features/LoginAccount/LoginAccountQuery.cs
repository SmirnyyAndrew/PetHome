using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.LoginAccount;
public record LoginAccountQuery(string Login, string Password) : IQuery;
