using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
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
using PetHome.SharedKernel.Options.Accounts;

namespace PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public class UpdateAccessTokenUsingRefreshTokenUseCase
    : ICommandHandler<LoginResponse, UpdateAccessTokenUsingRefreshTokenCommand>
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IValidator<UpdateAccessTokenUsingRefreshTokenCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    private readonly ICacheService _cache;
    private readonly RefreshTokenOption _refreshTokenOption;

    public UpdateAccessTokenUsingRefreshTokenUseCase(
        IAuthenticationRepository authenticationRepository,
        ITokenProvider tokenProvider,
        ICacheService cache, 
        IConfiguration configuration,
        IValidator<UpdateAccessTokenUsingRefreshTokenCommand> validator)
    { 
        _authenticationRepository = authenticationRepository;
        _validator = validator;
        _tokenProvider = tokenProvider;
        _cache = cache;
        _refreshTokenOption = configuration.GetSection(RefreshTokenOption.SECTION_NAME).Get<RefreshTokenOption>() ?? new();
    }

    public async Task<Result<LoginResponse, ErrorList>> Execute(
        UpdateAccessTokenUsingRefreshTokenCommand command,
        CancellationToken ct)
    {
        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is not true)
            return validationResult.Errors.ToErrorList();

        var cachedRefreshSession = await _cache.GetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, ct);
        if (cachedRefreshSession is null)
            return Errors.Failure($"Refresh token is expired").ToErrorList(); 

        RefreshSession oldRefreshSession = cachedRefreshSession;
        if (oldRefreshSession.ExpiredIn < DateTime.UtcNow)
            return Errors.Failure("Access token is expired").ToErrorList();

        Guid userId = oldRefreshSession.UserId;
        var getUserFromDbResult = await _authenticationRepository.GetUserById(userId, ct);
        if (getUserFromDbResult.IsFailure)
            return getUserFromDbResult.Error.ToErrorList();
         
        User user = getUserFromDbResult.Value;
        await _cache.RemoveAsync(Constants.Redis.REFRESH_TOKEN, ct); 

        var generateRefreshTokenResul = _tokenProvider.GenerateRefreshToken(user, oldRefreshSession);
        if (generateRefreshTokenResul.IsFailure)
            return generateRefreshTokenResul.Error;
      
        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromDays(_refreshTokenOption.ExpiredDays)
        };
        RefreshSession newRefreshSession = generateRefreshTokenResul.Value;
        await _cache.SetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, newRefreshSession, cacheOptions, ct); 

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
