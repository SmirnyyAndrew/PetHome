using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.UserManagment;
public interface ICreateParticipantContract
{
    public Task<UserId> Execute(Email email, UserName userName, CancellationToken ct);
}
