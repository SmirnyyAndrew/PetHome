using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.RegisterAccount;
public class RegisterAccountUseCase
    : ICommandHandler<string, RegisterAccountCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public RegisterAccountUseCase(
        UserManager<User> userManager,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Execute(
        RegisterAccountCommand command,
        CancellationToken ct)
    {

        var userIsExist = await _userManager.FindByNameAsync(command.Login);
        if (userIsExist is not null)
            return Errors.Conflict($"Пользователь с логином = {command.Login}").ToErrorList();

        User user = new User()
        {
            UserName = command.Login
        };

        var result = await _userManager.CreateAsync(user, command.Password);  
        var token = await _tokenProvider.GenerateToken(user, ct);
        return token;
    }
}
