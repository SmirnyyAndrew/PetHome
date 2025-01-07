using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Tokens.RefreshToken;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Application;
public interface ITokenProvider
{
    public Result<RefreshSession, Error> GenerateRefreshToken(User user, string accessToken);
    public string GenerateAccessToken(User user);
}
