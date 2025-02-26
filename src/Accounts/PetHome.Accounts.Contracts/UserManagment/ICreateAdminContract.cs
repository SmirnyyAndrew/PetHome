using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.UserManagment;
public interface ICreateAdminContract
{
    public Task<Result<UserId, Error>> Execute(Email email, UserName userName, CancellationToken ct);
}
