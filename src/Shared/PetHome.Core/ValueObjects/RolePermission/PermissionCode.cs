using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;

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
            return Errors.Validation("Permission code");

        return new PermissionCode(value);
    }

    public static implicit operator string(PermissionCode code) => code.Value;
}
