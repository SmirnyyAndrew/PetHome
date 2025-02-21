using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Models;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Domain.Accounts;
public class AdminAccount : DomainEntity<Guid>, ISoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("admin").Value;
     
    public UserId UserId { get; private set; }
    public User User { get; private set; } 
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    private AdminAccount(UserId userId) : base(userId)
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
     
    public void SoftDelete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }

    public void SoftRestore()
    {
        DeletionDate = default;
        IsDeleted = false;
    }
}
