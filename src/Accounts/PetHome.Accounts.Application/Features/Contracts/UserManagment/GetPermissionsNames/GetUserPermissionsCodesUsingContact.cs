using CSharpFunctionalExtensions;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.UserManagment;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.GetPermissions;
public class GetUserPermissionsCodesUsingContact
    (IAuthenticationRepository repository) : IGetUserPermissionsCodesContact
{
    public async Task<IReadOnlyList<string>> Execute(
        Guid userId, CancellationToken ct)
    {
        var result = await repository.GetUserPermissions(userId, ct);
        if (result.IsFailure)
            return [];

        IReadOnlyList<string> permissionsNames = result.Value.Select(p => p.Code.Value).ToList();
        return permissionsNames;
    }
}
