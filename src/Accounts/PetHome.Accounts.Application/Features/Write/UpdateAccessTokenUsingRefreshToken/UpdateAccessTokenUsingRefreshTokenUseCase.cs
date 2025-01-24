
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Dto;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenUseCase
    : ICommandHandler<TokenResponse, UpdateAccessTokenUsingRefreshTokenCommand>
{
    private readonly IRefreshSessionRepository _refreshTokenRepository;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IValidator<UpdateAccessTokenUsingRefreshTokenCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccessTokenUsingRefreshTokenUseCase(
        IRefreshSessionRepository refreshTokenRepository,
        IAuthenticationRepository authenticationRepository,
        IValidator<UpdateAccessTokenUsingRefreshTokenCommand> validator,
        ITokenProvider tokenProvider,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _authenticationRepository = authenticationRepository;
        _validator = validator;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TokenResponse, ErrorList>> Execute(
        UpdateAccessTokenUsingRefreshTokenCommand command,
        CancellationToken ct)
    {
        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is not true)
            return validationResult.Errors.ToErrorList();

        var refreshSessionResult = await _refreshTokenRepository.GetByRefreshToken(command.RefreshToken, ct);
        if (refreshSessionResult.IsFailure)
            return refreshSessionResult.Error.ToErrorList();

        RefreshSession oldRefreshSession = refreshSessionResult.Value;
        if (oldRefreshSession.ExpiredIn < DateTime.UtcNow)
            return Errors.Failure("Access token is expired").ToErrorList();

        var getUserIdFromTokenResult = _tokenProvider.GetUserId(command.AccessToken);
        if (getUserIdFromTokenResult.IsFailure)
            return refreshSessionResult.Error.ToErrorList();

        Guid userId = getUserIdFromTokenResult.Value;
        var getUserFromDbResult = await _authenticationRepository.GetUserById(userId, ct);
        if (getUserFromDbResult.IsFailure)
            return refreshSessionResult.Error.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        User user = getUserFromDbResult.Value;
        await _refreshTokenRepository.RemoveOldWithSavingChanges(user, ct);

        var generateRefreshTokenResul = _tokenProvider.GenerateRefreshToken(user, command.AccessToken);
        if (generateRefreshTokenResul.IsFailure)
            return generateRefreshTokenResul.Error;

        RefreshSession newRefreshSession = generateRefreshTokenResul.Value;
        await _refreshTokenRepository.Add(newRefreshSession, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        string newJwtTokenString = _tokenProvider.GenerateAccessToken(user);
        string newRefreshToken = newRefreshSession.RefreshToken.ToString();

        TokenResponse tokenResponse = new TokenResponse(newJwtTokenString, newRefreshToken);
        return tokenResponse;
    }
}
