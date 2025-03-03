using CSharpFunctionalExtensions;  
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.EmailManagement.ConfirmEmail;
public class ConfirmEmailUseCase
    : ICommandHandler<ConfirmEmailCommand>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailUseCase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        ConfirmEmailCommand command, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null)
            return Errors.NotFound("user").ToErrorList();
        
        var result = await _userManager.ConfirmEmailAsync(user, command.Token);
        if (result.Succeeded is false)
            return Errors.Failure("Не удалось подтвердить почту").ToErrorList();

        return Result.Success<ErrorList>();
    }
}
