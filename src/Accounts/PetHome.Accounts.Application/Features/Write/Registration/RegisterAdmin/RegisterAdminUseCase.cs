using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterAccount;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterVolunteerAccount;
public class RegisterAdminUseCase
    : ICommandHandler<AdminAccount, RegisterUserCommand>
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterAdminUseCase(
        [FromServices] RegisterUserUseCase registerUserUseCase,
        IAuthenticationRepository repository,
        IUnitOfWork unitOfWork
        )
    {
        _registerUserUseCase = registerUserUseCase;
        _repository = repository;
        _unitOfWork = unitOfWork;
    } 

    public async Task<Result<AdminAccount, ErrorList>> Execute(
        RegisterUserCommand command, CancellationToken ct)
    {
        var result = await _registerUserUseCase.Execute(command, ct);
        if (result.IsFailure)
            return result.Error;

        User user = result.Value;
        AdminAccount admin = AdminAccount.Create(user).Value;
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _repository.AddAdmin(admin, ct);
            await _unitOfWork.SaveChanges(ct);
            transaction.Commit();

            return admin;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _repository.RemoveUser(user);
            await _unitOfWork.SaveChanges(ct);
            throw;
        }
    }
}
