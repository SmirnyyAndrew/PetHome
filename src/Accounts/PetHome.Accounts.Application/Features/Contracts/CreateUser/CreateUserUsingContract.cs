using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Contracts.CreateUser;
public class CreateUserUsingContract : ICreateUserContract
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserUsingContract(
        IAuthenticationRepository repository,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserId> Execute(
        Email email, UserName userName, RoleId roleId, CancellationToken ct)
    {
        Role role = _repository.GetRole(roleId.Value).Result.Value;
        User user = User.Create(email, userName, role).Value;
        UserId userId = UserId.Create(user.Id).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        transaction.Commit();
        await _unitOfWork.SaveChanges(ct);

        return userId;
    }
}
