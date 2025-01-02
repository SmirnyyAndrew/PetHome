using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class PermissionId
{
    private Guid Value { get; set; }
    public PermissionId(Guid value)
    {
        Value = value;
    }

    public static Result<PermissionId, Error> Create(Guid value)
    {
        return new PermissionId(value);
    }
}
