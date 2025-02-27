namespace PetHome.Accounts.Contracts.Contracts.UserManagment;
public interface IGetUserPermissionsCodesContact
{
    public Task<IReadOnlyList<string>> Execute(Guid userId, CancellationToken ct);
}
