using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
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
