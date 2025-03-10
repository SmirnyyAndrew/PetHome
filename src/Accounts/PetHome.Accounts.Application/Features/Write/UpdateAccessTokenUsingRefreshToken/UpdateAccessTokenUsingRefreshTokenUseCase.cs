
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Login;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenUseCase
    : ICommandHandler<LoginResponse, UpdateAccessTokenUsingRefreshTokenCommand>
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IValidator<UpdateAccessTokenUsingRefreshTokenCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    private readonly ICacheService _cache;
    //private readonly IUnitOfWork _unitOfWork;
    //private readonly IRefreshSessionRepository _refreshTokenRepository;

    public UpdateAccessTokenUsingRefreshTokenUseCase(
        IAuthenticationRepository authenticationRepository,
        ITokenProvider tokenProvider,
        ICacheService cache,
        //IRefreshSessionRepository refreshTokenRepository,
        //[FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IValidator<UpdateAccessTokenUsingRefreshTokenCommand> validator)
    {
        //_refreshTokenRepository = refreshTokenRepository;
        //_unitOfWork = unitOfWork;
        _authenticationRepository = authenticationRepository;
        _validator = validator;
        _tokenProvider = tokenProvider;
        _cache = cache;
    }

    public async Task<Result<LoginResponse, ErrorList>> Execute(
        UpdateAccessTokenUsingRefreshTokenCommand command,
        CancellationToken ct)
    {
        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is not true)
            return validationResult.Errors.ToErrorList();

        var CachedRefreshSession = await _cache.GetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, ct);
        if (CachedRefreshSession is null)
            return Errors.Failure($"Refresh token is expired").ToErrorList();

        //var refreshSessionResult = await _refreshTokenRepository.GetByRefreshToken(command.RefreshToken, ct);
        //if (refreshSessionResult.IsFailure)
        //    return refreshSessionResult.Error.ToErrorList();

        RefreshSession oldRefreshSession = CachedRefreshSession;
        if (oldRefreshSession.ExpiredIn < DateTime.UtcNow)
            return Errors.Failure("Access token is expired").ToErrorList();

        Guid userId = oldRefreshSession.UserId;
        var getUserFromDbResult = await _authenticationRepository.GetUserById(userId, ct);
        if (getUserFromDbResult.IsFailure)
            return getUserFromDbResult.Error.ToErrorList();

        //var transaction = await _unitOfWork.BeginTransaction(ct);
        User user = getUserFromDbResult.Value;
        await _cache.RemoveAsync(Constants.Redis.REFRESH_TOKEN, ct);
        //await _refreshTokenRepository.RemoveOldWithSavingChanges(user, ct);

        var generateRefreshTokenResul = _tokenProvider.GenerateRefreshToken(user, oldRefreshSession);
        if (generateRefreshTokenResul.IsFailure)
            return generateRefreshTokenResul.Error;
      
        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromDays(7)
        };
        RefreshSession newRefreshSession = generateRefreshTokenResul.Value;
        await _cache.SetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, newRefreshSession, cacheOptions, ct);
        //await _refreshTokenRepository.Remove(oldRefreshSession, ct);
        //await _refreshTokenRepository.Add(newRefreshSession, ct);
        //await _unitOfWork.SaveChanges(ct);
        //transaction.Commit();

        string newJwtTokenString = _tokenProvider.GenerateAccessToken(user);
        string newRefreshToken = newRefreshSession.RefreshToken.ToString();

        LoginResponse loginResponse = new LoginResponse(newJwtTokenString,
            newRefreshToken,
            user.Id.ToString(),
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty);
        return loginResponse;
    }
}
