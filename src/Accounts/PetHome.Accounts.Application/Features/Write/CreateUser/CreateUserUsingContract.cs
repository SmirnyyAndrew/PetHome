using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Contracts.UserManagment;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.CreateUser;
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

    public async Task<Result<UserId, Error>> Execute(RoleId roleId, CancellationToken ct)
    {
        Email email = Email.Create("Emas2fgoiL123@mail.com").Value;
        UserName userName = UserName.Create("Ivanov Ivan").Value;

        var geRoleResult = await _repository.GetRole(roleId.Value);
        if (geRoleResult.IsFailure)
            return geRoleResult.Error;

        Role role = geRoleResult.Value;
        User user = User.Create(email, userName, role).Value;
        UserId userId = UserId.Create(user.Id).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return userId;
    }
}
