using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.UserManagment;
public interface ICreateVolunteerAccountContract
{
    public Task<UserId> Execute(
        Email email,
        UserName userName,
        Date startVolunteeringDate,
        IReadOnlyList<Requisites> requisites,
        IReadOnlyList<Certificate> certificates,
        CancellationToken ct);
}
