using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.LoginUser;
public record LoginUserQuery(string Email, string Password) : IQuery;
