using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.Response.Validation.Validator;

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
