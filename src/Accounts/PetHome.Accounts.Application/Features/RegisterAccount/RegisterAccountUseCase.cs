using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
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

        var userIsExist = await _userManager.FindByEmailAsync(command.Email);
        if (userIsExist is not null)
            return Errors.Conflict($"Пользователь с email = {command.Email}").ToErrorList();

        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.Name).Value;

        User user = new User()
        {
            Email = email, 
            UserName = userName

        };

        var result = await _userManager.CreateAsync(user, command.Password);   
        if(result.Succeeded is false)
            return result.Errors.ToErrorList();

        return Result.Success<ErrorList>();
    }
}
