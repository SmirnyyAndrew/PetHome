using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class Permission
{
    public PermissionId Id { get; private set; }
    public PermissionCode Code { get; private set; }
    
    private Permission() { }
    public Permission(string value)
    {
        Id = PermissionId.Create().Value;
        Code = PermissionCode.Create(value).Value;
    }

    public static Result<Permission, Error> Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Errors.Validation("Permission");

        return new Permission(code);
    }
}
