namespace PetHome.Accounts.Contracts.Contracts.UserManagment;
public interface IGetUserRoleNameContract
{
    public Task<string> Execute(Guid userId, CancellationToken ct);
}
