using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class Role : IdentityRole<Guid>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Role() { }
    public Role(string name)
    {
        Name = name;
    }

    public static Result<Role, Error> Create(string name)
    {
        return new Role(name);
    }
}
