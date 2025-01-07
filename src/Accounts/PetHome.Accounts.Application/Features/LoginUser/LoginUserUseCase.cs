using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.LoginUser;
public class LoginUserUseCase
    : IQueryHandler<string, LoginUserQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Execute(
        LoginUserQuery query,
        CancellationToken ct)
    {
        ErrorList error = Errors.NotFound("User").ToErrorList();

        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user == null)
            return error;

        var passwordIsConfirmed = await _userManager.CheckPasswordAsync(user!, query.Password);
        if(passwordIsConfirmed is false)
            return error;

        var token = _tokenProvider.GenerateAccessToken(user);
        return token;
    }
}
