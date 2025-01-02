using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class Permission
{
    private PermissionId Id { get; set; }
    private PermissionCode Code { get; set; }
    public Permission(string value)
    {
        Id = PermissionId.Create().Value;
        Code = PermissionCode.Create(value).Value;
    }

    public static Result<Permission, Error> Create(string code)
    {
        if (!string.IsNullOrWhiteSpace(code))
            return Errors.Validation("Permission");

        return new Permission(code);
    }
}
