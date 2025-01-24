using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts;
public interface ICreateUserContract
{
    public Task<UserId> Execute(RoleId roleId, CancellationToken ct);
}
