using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.LoginAccount;
public class LoginAccountUseCase
    : IQueryHandler<string, LoginAccountQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public LoginAccountUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Execute(
        LoginAccountQuery query,
        CancellationToken ct)
    {
        ErrorList error = Errors.NotFound("User").ToErrorList();

        var user = await _userManager.FindByNameAsync(query.Login);
        if (user == null)
            return error;

        var passwordIsConfirmed = await _userManager.CheckPasswordAsync(user!, query.Password);
        if(passwordIsConfirmed == false)
            return error;

        var token = await _tokenProvider.GenerateToken(user, ct);
        return token;
    }
}
