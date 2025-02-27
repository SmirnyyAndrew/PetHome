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

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement;
public class CreateAdminConsumer : IConsumer<CreatedAdminEvent>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatedAdminEvent> _validator;
    private readonly ILogger<CreateAdminConsumer> _logger;

    public CreateAdminConsumer(
        IAuthenticationRepository repository,
        IValidator<CreatedAdminEvent> validator,
        ILogger<CreateAdminConsumer> logger,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }
    public async Task Consume(ConsumeContext<CreatedAdminEvent> context)
    {
        var command = context.Message;

        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is not true)
        {
            _logger.LogError("Ошибка валидации");
            return;
        }

        var geRoleResult = await _repository.GetRole(AdminAccount.ROLE);
        if (geRoleResult.IsFailure)
        {
            _logger.LogError("Не найдена роль админа");
            return;
        }

        Role role = geRoleResult.Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, role, avatar: null, id: command.Id).Value;
        AdminAccount admin = AdminAccount.Create(user).Value;


        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.AddUser(user, CancellationToken.None);
        await _repository.AddAdmin(admin, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit(); 
        return;
    }
}
