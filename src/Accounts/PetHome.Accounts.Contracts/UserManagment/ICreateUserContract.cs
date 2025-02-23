using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.UserManagment;
public interface ICreateUserContract
{
    public Task<Result<UserId, Error>> Execute(RoleId roleId, CancellationToken ct);
}
