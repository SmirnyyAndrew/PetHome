using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
public interface IGenerateAccessTokenContract
{
    public Task<Result<string, Error>> Execute(UserId userId, CancellationToken ct);
}
