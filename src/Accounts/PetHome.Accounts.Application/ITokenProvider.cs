using PetHome.Accounts.Domain;

namespace PetHome.Accounts.Application;
public interface ITokenProvider
{
    public Task<string> GetToken(User user, CancellationToken ct);
}
