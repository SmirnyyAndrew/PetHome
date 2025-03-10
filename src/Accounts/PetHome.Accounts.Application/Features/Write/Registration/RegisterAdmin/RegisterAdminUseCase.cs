using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterUser;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterAdmin;
public class RegisterAdminUseCase
    : ICommandHandler<RegisterAdminCommand>
{
    private readonly IPublishEndpoint _publisher;
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork; 

    public RegisterAdminUseCase(
        IAuthenticationRepository repository,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)]
        IUnitOfWork unitOfWork,
        IPublishEndpoint publisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterAdminCommand command, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        User user = getUserResult.Value;
        AdminAccount admin = AdminAccount.Create(user).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddAdmin(admin, ct);
        await _unitOfWork.SaveChanges(ct);
         
        CreatedAdminEvent createdAdminEvent = new CreatedAdminEvent(
            user.Id, user.Email, user.UserName);
        await _publisher.Publish(createdAdminEvent);
        transaction.Commit(); 

        return Result.Success<ErrorList>();
    }
}
