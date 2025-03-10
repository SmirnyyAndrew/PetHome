using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Redis;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Login;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.LoginUser;
public class LoginUserUseCase
    : IQueryHandler<LoginResponse, LoginUserQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    //private readonly IRefreshSessionRepository _repository;
    //private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public LoginUserUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
          //IRefreshSessionRepository repository, 
          //[FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
          ICacheService cache)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        //_repository = repository;
        //_unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result<LoginResponse, ErrorList>> Execute(
        LoginUserQuery query,
        CancellationToken ct)
    {
        ErrorList error = Errors.NotFound("User").ToErrorList(); 

        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user is null)
            return error;

        //await _repository.RemoveOldWithSavingChanges(user, ct);
        await _cache.RemoveAsync(Constants.Redis.REFRESH_TOKEN, ct);

        var passwordIsConfirmed = await _userManager.CheckPasswordAsync(user!, query.Password);
        if (passwordIsConfirmed is false)
            return error;

        var jwtToken = _tokenProvider.GenerateAccessToken(user);
        var refreshSession = _tokenProvider.GenerateRefreshToken(user, jwtToken).Value;
        Guid refreshToken = refreshSession.RefreshToken;

        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromDays(7)
        };
        await _cache.SetAsync<RefreshSession>(Constants.Redis.REFRESH_TOKEN, refreshSession, cacheOptions, ct);
        //var transaction = await _unitOfWork.BeginTransaction(ct);
        //await _repository.Add(refreshSession, ct);
        //await _unitOfWork.SaveChanges(ct);
        //transaction.Commit();

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
