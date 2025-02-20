using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.UserManagment;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.GetRoles;
public class GetUserRoleNameUsingContract
    (IAuthenticationRepository repository) : IGetUserRoleNameContract
{
    public async Task<string> Execute(
        Guid userId, CancellationToken ct)
    {
        var result = await repository.GetUserRole(userId, ct);
        if (result.IsFailure)
            return string.Empty;

        string userRoleName = result.Value.Name;
        return userRoleName;    
    }
}
