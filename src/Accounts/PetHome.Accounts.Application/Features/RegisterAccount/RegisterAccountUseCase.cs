using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
namespace PetHome.Accounts.Application.Features.RegisterAccount;
public class RegisterAccountUseCase
    : ICommandHandler<RegisterAccountCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public RegisterAccountUseCase(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterAccountCommand command,
        CancellationToken ct)
    {

        var userIsExist = await _userManager.FindByEmailAsync(command.Email);
        if (userIsExist is not null)
            return Errors.Conflict($"Пользователь с email = {command.Email}").ToErrorList();

        string roleName = "Volunteer";
        RoleId? roleId = RoleId.Create(_roleManager.Roles.FirstOrDefault(r => r.Name == roleName).Id).Value;

        User user = new User()
        {
            Email = command.Email,
            UserName = command.Name,
            RoleId = roleId

        };

        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded is false)
            return result.Errors.ToErrorList();

        return Result.Success<ErrorList>();
    }
}
