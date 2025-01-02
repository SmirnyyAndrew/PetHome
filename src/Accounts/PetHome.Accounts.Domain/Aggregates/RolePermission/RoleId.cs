using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class RoleId : IdentityRole<Guid>
{
    private Guid Value { get; set; }
    public RoleId(Guid value)
    {
        Value = value;
    }

    public static Result<RoleId, Error> Create(Guid value)
    {
        return new RoleId(value);
    }
}
