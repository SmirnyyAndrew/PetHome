using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.User;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.CreateUser;
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

    public async Task<UserId> Execute(RoleId roleId, CancellationToken ct)
    {
        Email email = Email.Create("Emas2fgoiL123@mail.com").Value;
        UserName userName = UserName.Create("Ivanov Ivan").Value;
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
