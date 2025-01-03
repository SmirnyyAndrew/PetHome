using CSharpFunctionalExtensions;
using PetHome.SharedKernel.ValueObjects.AuthAggregates.User;

namespace PetHome.Accounts.Domain.Accounts;
public class AdminAccount
{
    public UserId? UserId { get; set; }

    private AdminAccount() { }
    private AdminAccount(UserId userId)
    {
        UserId = userId;
    }

    public static AdminAccount Create(UserId userId)
    {
        return new AdminAccount(userId);
    }
}
