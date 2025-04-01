using AccountService.Application.Database.Repositories;
using AccountService.Contracts.Messaging.UserManagment;
using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace AccountService.Application.Features.Write.CreateAdmin;
public class CreateAdminUseCase
    : ICommandHandler<UserId, CreateAdminCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateAdminCommand> _validator;
    private readonly ILogger<CreateAdminUseCase> _logger;
    private readonly IPublishEndpoint _publisher;

    public CreateAdminUseCase(
        IAuthenticationRepository repository,
        IValidator<CreateAdminCommand> validator,
        ILogger<CreateAdminUseCase> logger,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<UserId, ErrorList>> Execute(CreateAdminCommand command, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is not true)
            return validationResult.Errors.ToErrorList();

        var geRoleResult = await _repository.GetRole(AdminAccount.ROLE);
        if (geRoleResult.IsFailure)
            return geRoleResult.Error.ToErrorList();

        Role role = geRoleResult.Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, role).Value;
        AdminAccount admin = AdminAccount.Create(user).Value;
         

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        await _repository.AddAdmin(admin, ct);
        await _unitOfWork.SaveChanges(ct);

        CreatedAdminEvent createdAdminEvent = new CreatedAdminEvent(
            user.Id, user.Email, user.UserName);
        await _publisher.Publish(createdAdminEvent, ct);  
        transaction.Commit();

        UserId userId = UserId.Create(user.Id).Value;
        return userId;
    }
}
