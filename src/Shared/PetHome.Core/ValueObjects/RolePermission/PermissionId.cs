using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.RolePermission;
public record PermissionId
{
    public Guid Value { get; }
    private PermissionId(Guid value)
    {
        Value = value;
    }

    public static Result<PermissionId, Error> Create(Guid value) => new PermissionId(value);

    public static Result<PermissionId, Error> Create() => new PermissionId(Guid.NewGuid());

    public static implicit operator Guid(PermissionId id) => id.Value;
}
