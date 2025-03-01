using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateUser;
public class CreateUserConsumer : IConsumer<CreatedUserEvent>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserConsumer> _logger;
    private readonly IValidator<CreatedUserEvent> _validator;

    public CreateUserConsumer(
        IAuthenticationRepository repository,
        ILogger<CreateUserConsumer> logger,
        IValidator<CreatedUserEvent> validator,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;

        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is false)
        {
            _logger.LogError("Ошибка валидации");
            return;
        }

        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;

        var geRoleResult = command.RoleId == default
            ? await _repository.GetRole(ParticipantAccount.ROLE)
            : await _repository.GetRole(command.RoleId);

        if (geRoleResult.IsFailure)
        {
            _logger.LogError("Не найдена роль");
            return;
        }

        Role role = geRoleResult.Value;
        User user = User.Create(email, userName, role).Value;
        UserId userId = UserId.Create(user.Id).Value;

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.AddUser(user, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;

    }
}
