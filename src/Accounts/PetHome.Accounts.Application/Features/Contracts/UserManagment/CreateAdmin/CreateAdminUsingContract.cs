using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.UserManagment;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Contracts.UserManagment.CreateAdmin;
internal class CreateAdminUsingContract : ICreateAdminContract
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAdminUsingContract(
        IAuthenticationRepository repository,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserId, Error>> Execute(Email email, UserName userName, CancellationToken ct)
    {
        var geRoleResult = await _repository.GetRole(AdminAccount.ROLE);
        if(geRoleResult.IsFailure)
            return geRoleResult.Error;

        Role role = geRoleResult.Value;
        User user = User.Create(email, userName, role).Value;
        AdminAccount admin = AdminAccount.Create(user).Value;


        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        await _repository.AddAdmin(admin, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        UserId userId = UserId.Create(user.Id).Value;
        return userId;
    }
}
