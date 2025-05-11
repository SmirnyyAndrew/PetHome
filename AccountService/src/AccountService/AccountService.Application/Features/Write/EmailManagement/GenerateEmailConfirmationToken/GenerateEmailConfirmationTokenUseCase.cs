using AccountService.Application.Database.Repositories;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
public class GenerateEmailConfirmationTokenUseCase
    : ICommandHandler<string, GenerateEmailConfirmationTokenCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationRepository _repository;

    public GenerateEmailConfirmationTokenUseCase(
        UserManager<User> userManager,
        IAuthenticationRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<Result<string, ErrorList>> Execute(
        GenerateEmailConfirmationTokenCommand command, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();
        User user = getUserResult.Value;
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return token;
    }
}
