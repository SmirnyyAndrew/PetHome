using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public record PermissionId
{
    public Guid Value { get; }
    public PermissionId(Guid value)
    {
        Value = value;
    }

    public static Result<PermissionId, Error> Create(Guid value)
    {
        return new PermissionId(value);
    }

    public static Result<PermissionId, Error> Create()
    {
        return new PermissionId(Guid.NewGuid());
    }
}
