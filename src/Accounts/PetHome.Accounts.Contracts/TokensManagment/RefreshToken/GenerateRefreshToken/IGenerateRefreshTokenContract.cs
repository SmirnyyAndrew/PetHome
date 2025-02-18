using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.TokensManagment.RefreshToken.GenerateRefreshToken;
public interface IGenerateRefreshTokenContract
{
    public Task<Result<RefreshSession, Error>> Execute(
        UserId userId, RefreshSession oldRefreshSession, CancellationToken ct);

    public Task<Result<RefreshSession, Error>> Execute(
        UserId userId, string accessToken, CancellationToken ct);
}
