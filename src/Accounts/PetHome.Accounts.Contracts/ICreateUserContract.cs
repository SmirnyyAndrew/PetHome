using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts;
public interface ICreateUserContract
{
    public Task<UserId> Execute(
        Email email, UserName userName, RoleId roleId, CancellationToken ct);
}
