using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Read.GetUserInformation;
public class GetUserInformationUseCase
    : IQueryHandler<User, GetUserInformationQuery>
{
    private readonly IAuthenticationRepository _repository;

    public GetUserInformationUseCase(IAuthenticationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<User, ErrorList>> Execute(
        GetUserInformationQuery query, CancellationToken ct)
    {
        var result = await _repository.GetUserById(query.UserId, ct);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        User user = result.Value;
        return user;
    }
}
