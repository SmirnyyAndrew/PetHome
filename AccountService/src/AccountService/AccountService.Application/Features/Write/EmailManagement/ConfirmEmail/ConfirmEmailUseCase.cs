using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Write.EmailManagement.ConfirmEmail;
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
