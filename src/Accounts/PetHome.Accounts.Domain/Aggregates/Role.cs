using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Domain.Aggregates;
public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
    public override string Name { get; set; }
    public IReadOnlyList<Permission> Permissions { get; set; } = [];

    private Role() { }
    public Role(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public static Result<Role, Error> Create(string name)
    {
        return new Role(name);
    }

    public void SetPermissions(IEnumerable<Permission> permissions)
    {
        Permissions = permissions.ToList();
    }
}
