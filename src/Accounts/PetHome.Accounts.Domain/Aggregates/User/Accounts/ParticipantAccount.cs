using CSharpFunctionalExtensions;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class ParticipantAccount
{
    public static string ROLE = "participant";
    public UserId UserId { get; set; }
    public IReadOnlyList<Pet>? FavoritePets { get; private set; }

    private ParticipantAccount() { }
    private ParticipantAccount(UserId userId)
    {
        UserId = userId;
    }

    public static Result<ParticipantAccount, Error> Create(User user)
    {
        Role role = user.Role;
        if (role.Name.ToLower() == ROLE)
        {
            UserId userId = UserId.Create(user.Id).Value;
            return new ParticipantAccount(userId);
        }
        return Errors.Conflict($"пользователь с id = {user.Id}");
    }
}
