using CSharpFunctionalExtensions;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class AdminAccount
{
    public UserId UserId { get; set; }

    private AdminAccount() { }
    private AdminAccount(UserId userId)
    {
        UserId = userId;
    }

    public static Result<AdminAccount> Create(UserId userId)
    {
        return new AdminAccount(userId);
    }
}
