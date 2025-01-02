using PetHome.Accounts.Domain;

namespace PetHome.Accounts.Application;
public interface ITokenProvider
{
    public Task<string> GenerateToken(User user, CancellationToken ct);
}
