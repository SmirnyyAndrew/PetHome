using AccountService.Application.Database.Repositories;
using AccountService.Contracts.Messaging.UserManagement;
using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Write.Registration.RegisterAdmin;
public class RegisterAdminUseCase
    : ICommandHandler<RegisterAdminCommand>
{
    private readonly IPublishEndpoint _publisher;
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAdminUseCase(
        IAuthenticationRepository repository,
        [FromKeyedServices(Constants.Database.ACCOUNT_UNIT_OF_WORK_KEY)]
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
