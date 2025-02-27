using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
public class GenerateAccessTokenUsingContract : IGenerateAccessTokenContract
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthenticationRepository _repository;

    public GenerateAccessTokenUsingContract(
        ITokenProvider tokenProvider,
        IAuthenticationRepository repository)
    {
        _tokenProvider = tokenProvider;
        _repository = repository;
    }

    public async Task<Result<string, Error>> Execute(UserId userId, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(userId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error;

        User user = getUserResult.Value;

        string token = _tokenProvider.GenerateAccessToken(user);
        return token;
    }
}
