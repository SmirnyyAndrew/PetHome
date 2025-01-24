using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Contracts;
public interface IGetRoleContract
{
    public Task<RoleId> Execute(string name, CancellationToken ct);
}
