using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class ParticipantAccount
{
    public UserId? UserId { get; private set; }
    public IReadOnlyList<Pet>? FavoritePets { get; private set; }

    private ParticipantAccount() { }
    private ParticipantAccount(UserId userId)
    {
        UserId = userId;
    }

    public static ParticipantAccount Create(UserId userId)
    {
        return new ParticipantAccount(userId);
    }
}
