using AccountService.Application.Database.Repositories;
using AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace AccountService.Application.Features.Contracts.UserManagment.GetRolesNames;
public class GetUserRoleNameUseCase(IAuthenticationRepository repository)
    : IQueryHandler<string, GetUserRoleNameQuery>
{
    public async Task<Result<string, ErrorList>> Execute(
        GetUserRoleNameQuery query, CancellationToken ct)
    {
        var result = await repository.GetUserRole(query.UserId, ct);
        if (result.IsFailure)
            return string.Empty;

        string userRoleName = result.Value.Name;
        return userRoleName;
    }
}
