using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Contracts.TokensManagment.RefreshToken.GenerateRefreshToken;
public class GenerateRefreshTokenUsingContract : IGenerateRefreshTokenContract
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthenticationRepository _repository;
    private readonly IGenerateAccessTokenContract _generateAccessTokenContract;

    public GenerateRefreshTokenUsingContract(
        ITokenProvider tokenProvider,
        IAuthenticationRepository repository,
        IGenerateAccessTokenContract generateAccessTokenContract)
    {
        _tokenProvider = tokenProvider;
        _repository = repository;
        _generateAccessTokenContract = generateAccessTokenContract;
    }

    public async Task<Result<RefreshSession, Error>> Execute(UserId userId, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(userId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error;

        User user = getUserResult.Value;
        var generateAccessTokenResult = await _generateAccessTokenContract.Execute(userId, ct);
        if(generateAccessTokenResult.IsFailure)
            return generateAccessTokenResult.Error;

        string accessToken = generateAccessTokenResult.Value;


        var generateRefreshTokenResult =  _tokenProvider.GenerateRefreshToken(user, accessToken);
        if(generateRefreshTokenResult.IsFailure)
            return generateRefreshTokenResult.Error.Errors.First();

        RefreshSession refreshSession = generateRefreshTokenResult.Value; 
        return refreshSession;
    }
}
