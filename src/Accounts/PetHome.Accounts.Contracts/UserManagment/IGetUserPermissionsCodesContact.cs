namespace PetHome.Accounts.Contracts.UserManagment;
public interface IGetUserPermissionsCodesContact
{
    public Task<IReadOnlyList<string>> Execute(Guid userId, CancellationToken ct);
}
