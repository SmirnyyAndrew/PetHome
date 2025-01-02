using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PetHome.Accounts.Domain;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using System.Net;

namespace PetHome.Accounts.Application.Features.RegisterAccount;
public class RegisterAccountUseCase
    : ICommandHandler<RegisterAccountCommand>
{
    private readonly UserManager<User> _userManager; 

    public RegisterAccountUseCase(
        UserManager<User> userManager, 
        IConfiguration configuration)
    {
        _userManager = userManager; 
    }

    public async Task<UnitResult<ErrorList>> Execute(
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
        if(result.Succeeded is false)
            return result.Errors.ToErrorList();

        return Result.Success<ErrorList>();
    }
}
