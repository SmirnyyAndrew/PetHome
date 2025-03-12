using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
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

namespace PetHome.Accounts.Application.Features.Write.LoginUser;
public class LoginUserUseCase
    : IQueryHandler<LoginResponse, LoginUserQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ICacheService _cache;
    private readonly RefreshTokenOption _refreshTokenOption;

    public LoginUserUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        IConfiguration configuration,
        ICacheService cache)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _cache = cache;
        _refreshTokenOption = configuration.GetSection(RefreshTokenOption.SECTION_NAME).Get<RefreshTokenOption>() ?? new();
    }

    public async Task<Result<LoginResponse, ErrorList>> Execute(
        LoginUserQuery query,
        CancellationToken ct)
    {
        ErrorList error = Errors.NotFound("User").ToErrorList();

        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user is null)
            return error;

        await _cache.RemoveAsync(Constants.Redis.REFRESH_TOKEN, ct);

        var passwordIsConfirmed = await _userManager.CheckPasswordAsync(user!, query.Password);
        if (passwordIsConfirmed is false)
            return error;

        var jwtToken = _tokenProvider.GenerateAccessToken(user);
        var refreshSession = _tokenProvider.GenerateRefreshToken(user, jwtToken).Value;
        Guid refreshToken = refreshSession.RefreshToken;

        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromDays(_refreshTokenOption.ExpiredDays)
        };
        await _cache.SetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, refreshSession, cacheOptions, ct);

        var cachedRS = await _cache.GetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, ct);

        LoginResponse loginResponse = new LoginResponse(
            jwtToken,
            refreshToken.ToString(),
            user.Id.ToString(),
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty);
        return loginResponse;
    }
}
