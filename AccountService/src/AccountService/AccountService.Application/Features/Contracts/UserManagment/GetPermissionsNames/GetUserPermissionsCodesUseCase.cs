using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Contracts.UserManagment.GetPermissionsNames;
public class GetUserPermissionsCodesUseCase(IAuthenticationRepository repository)
    : IQueryHandler<IReadOnlyList<string>, GetUserPermissionsCodesQuery>
{
    public async Task<Result<IReadOnlyList<string>, ErrorList>> Execute(
        GetUserPermissionsCodesQuery query, CancellationToken ct)
    {
        var result = await repository.GetUserPermissions(query.UserId, ct);
        if (result.IsFailure)
            return new string[] { };

        var permissionsNames = result.Value.Select(p => p.Code.Value).ToList();
        return permissionsNames;
    }
}
