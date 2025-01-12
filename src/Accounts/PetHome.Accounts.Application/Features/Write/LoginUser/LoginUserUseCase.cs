using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Dto;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.LoginUser;
public class LoginUserUseCase
    : IQueryHandler<TokenResponse, LoginUserQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshSessionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        IRefreshSessionRepository repository,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TokenResponse, ErrorList>> Execute(
        LoginUserQuery query,
        CancellationToken ct)
    {
        ErrorList error = Errors.NotFound("User").ToErrorList();

        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user == null)
            return error;

        await _repository.RemoveOldWithSavingChanges(user, ct);

        var passwordIsConfirmed = await _userManager.CheckPasswordAsync(user!, query.Password);
        if (passwordIsConfirmed is false)
            return error;

        var jwtToken = _tokenProvider.GenerateAccessToken(user);
        var refreshSession = _tokenProvider.GenerateRefreshToken(user, jwtToken).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.Add(refreshSession, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        var refreshToken = refreshSession.RefreshToken;
        TokenResponse tokenResponse = new TokenResponse(jwtToken, refreshToken.ToString());
        return tokenResponse;
    }
}
