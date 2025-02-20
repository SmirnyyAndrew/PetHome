namespace PetHome.Accounts.Contracts.UserManagment;
public interface IGetUserRoleNameContract
{
    public Task<string> Execute(Guid userId, CancellationToken ct);
}
