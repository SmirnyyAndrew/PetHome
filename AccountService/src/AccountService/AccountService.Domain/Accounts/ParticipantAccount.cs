using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.Database;
using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Domain.Accounts;
public class ParticipantAccount : DomainEntity<Guid>, ISoftDeletableEntity
{
    public static RoleName ROLE = RoleName.Create("participant").Value;

    public UserId UserId { get; private set; }
    public User User { get; private set; }
    //public IReadOnlyList<Pet>? FavoritePets { get; private set; } 
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    private ParticipantAccount(UserId userId) : base(userId)
    {
        UserId = userId;
    }

    public static Result<ParticipantAccount, Error> Create(User user)
    {
        Role? role = user.Role;
        if (role is not null && role.Name.ToLower() == ROLE)
        {
            UserId userId = UserId.Create(user.Id).Value;
            return new ParticipantAccount(userId);
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
