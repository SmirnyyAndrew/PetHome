using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.RolePermission;
public record PermissionCode
{
    public string Value { get; }
    private PermissionCode(string value)
    {
        Value = value;
    }

    public static Result<PermissionCode, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("PermissionCode");

        return new PermissionCode(value);
    }
}
