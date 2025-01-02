using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public record PermissionCode
{
    private string Code { get; }
    private PermissionCode(string value)
    {

    }

    public static Result<PermissionCode, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("PermissionCode");

        return new PermissionCode(value);
    }
}
