using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.Responses.RefreshToken;

namespace AccountService.Application.Features.Contracts.TokenManagment.GenerateRefreshToken;
public class GenerateRefreshTokenUseCase
    : ICommandHandler<RefreshSession, GenerateRefreshTokenCommand>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthenticationRepository _repository;
    //private readonly GenerateAccessTokenUseCase _generateAccessTokenUseCase;

    public GenerateRefreshTokenUseCase(
        ITokenProvider tokenProvider,
        IAuthenticationRepository repository)
    {
        _tokenProvider = tokenProvider;
        _repository = repository;
        //_generateAccessTokenUseCase = generateAccessTokenUseCase;
    }

    public async Task<Result<RefreshSession, ErrorList>> Execute(
        GenerateRefreshTokenCommand command, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        User user = getUserResult.Value;
        //var generateAccessTokenResult = await _generateAccessTokenUseCase.Execute(new(command.UserId), ct);
        //if (generateAccessTokenResult.IsFailure)
        //    return generateAccessTokenResult.Error;

        var generateRefreshTokenResult = _tokenProvider.GenerateRefreshToken(user, command.AccessToken);
        if (generateRefreshTokenResult.IsFailure)
            return generateRefreshTokenResult.Error;

        RefreshSession refreshSession = generateRefreshTokenResult.Value;
        return refreshSession;
    }
}
