using PetHome.Accounts.Domain.Aggregates.User;

namespace PetHome.Accounts.Application;
public interface ITokenProvider
{
    public Task<string> GenerateToken(User user, CancellationToken ct);
}
