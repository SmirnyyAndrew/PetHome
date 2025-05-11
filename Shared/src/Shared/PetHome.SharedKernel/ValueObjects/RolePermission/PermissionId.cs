using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.RolePermission;
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
