using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Accounts.Domain.Aggregates;

namespace PetHome.Accounts.Domain.Accounts;
public class AdminAccount : SoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("admin").Value;
     
    public UserId UserId { get; set; }
    public User User { get; set; }
    private AdminAccount() { }
    private AdminAccount(UserId userId)
    {
        UserId = userId; 
    }

    public static Result<AdminAccount, Error> Create(User user)
    {
        Role role = user.Role;
        if (role is not null && role.Name.ToLower() == ROLE)
        {
            UserId userId = UserId.Create(user.Id).Value;
            return new AdminAccount(userId);
        }
        return Errors.Conflict($"пользователь с id = {user.Id}");
    }

    public override void SoftDelete() => base.SoftDelete();

    public override void SoftRestore() => base.SoftRestore();
}
