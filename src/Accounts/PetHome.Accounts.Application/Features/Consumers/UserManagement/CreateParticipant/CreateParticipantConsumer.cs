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

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateParticipant;
public class CreateParticipantConsumer : IConsumer<CreatedParticipantEvent>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateParticipantConsumer> _logger;
    private readonly IValidator<CreatedParticipantEvent> _validator;

    public CreateParticipantConsumer(
        IAuthenticationRepository repository,
        ILogger<CreateParticipantConsumer> logger,
        IValidator<CreatedParticipantEvent> validator,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }


    public async Task Consume(ConsumeContext<CreatedParticipantEvent> context)
    {
        var command = context.Message;

        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is not true)
        {
            _logger.LogError("Ошибка валидации");
            return;
        }

        var geRoleResult = await _repository.GetRole(ParticipantAccount.ROLE);
        if (geRoleResult.IsFailure)
        {
            _logger.LogError("Не найдена роль админа");
            return;
        }

        Role role = geRoleResult.Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, role, avatar: null, id: command.Id).Value;
        ParticipantAccount participant = ParticipantAccount.Create(user).Value;

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.AddUser(user, CancellationToken.None);
        await _repository.AddParticipant(participant, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
