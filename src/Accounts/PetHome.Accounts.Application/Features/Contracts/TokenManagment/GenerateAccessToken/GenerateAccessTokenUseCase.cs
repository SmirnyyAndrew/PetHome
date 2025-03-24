using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.HttpCommunication.Requests.TokenManagement;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
public class GenerateAccessTokenUseCase 
    : ICommandHandler<string, GenerateAccessTokenCommand>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthenticationRepository _repository;

    public GenerateAccessTokenUseCase(
        ITokenProvider tokenProvider,
        IAuthenticationRepository repository)
    {
        _tokenProvider = tokenProvider;
        _repository = repository;
    }

    public async Task<Result<string, ErrorList>> Execute(GenerateAccessTokenCommand command, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        User user = getUserResult.Value;

        string token = _tokenProvider.GenerateAccessToken(user);
        return token;
    }
}
