using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class Permission
{
    private string Code { get; set; }
    public Permission(string value)
    {
        Code = value;
    }

    public static Result<Permission, Error> Create(string code)
    {
        if (!string.IsNullOrWhiteSpace(code))
            return Errors.Validation("Permission");

        return new Permission(code);
    }
}
