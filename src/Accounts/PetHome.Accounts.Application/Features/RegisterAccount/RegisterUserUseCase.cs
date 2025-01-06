using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
namespace PetHome.Accounts.Application.Features.RegisterAccount;
public class RegisterUserUseCase
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly UserManager<User> _userManager;

    public RegisterUserUseCase(
        IAuthenticationRepository repository,
        UserManager<User> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }


    public async Task<UnitResult<ErrorList>> Execute(
        RegisterUserCommand command,
        CancellationToken ct)
    {
        Email email = Email.Create(command.Email).Value;
        var userIsExist = await _repository.GetUserByEmail(email, ct);
        if (userIsExist.IsFailure)
            return userIsExist.Error.ToErrorList();

        Role role = _repository.GetRole(User.ROLE).Result.Value;
        RoleId roleId = RoleId.Create(role.Id).Value;
        UserName userName = UserName.Create(command.Name).Value;
        User user = User.Create(email, userName, roleId);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded is false)
            return result.Errors.ToErrorList();
       
        return Result.Success<ErrorList>();
    }
}
