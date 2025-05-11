using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
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
