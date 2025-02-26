using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.RolePermission;
public record RoleName
{
    public string Value { get; }
    private RoleName(string value)
    {
        Value = value;
    }

    public static Result<RoleName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("PermissionCode");

        return new RoleName(value);
    }

    public static implicit operator string(RoleName name) => name.Value;
}
