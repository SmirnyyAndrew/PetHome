using CSharpFunctionalExtensions;
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
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateVolunteerAccount;
public class CreateVolunteerAccountConsumer : IConsumer<CreatedVolunteerAccountEvent>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatedVolunteerAccountEvent> _logger;
    private readonly IValidator<CreatedVolunteerAccountEvent> _validator;

    public CreateVolunteerAccountConsumer(
        IAuthenticationRepository repository,
        ILogger<CreatedVolunteerAccountEvent> logger,
        IValidator<CreatedVolunteerAccountEvent> validator,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreatedVolunteerAccountEvent> context)
    {
        var command = context.Message;

        var validationResult = await _validator.ValidateAsync(command, CancellationToken.None);
        if (validationResult.IsValid is not true)
        {
            _logger.LogError("Ошибка валидации");
            return;
        }

        var geRoleResult = await _repository.GetRole(VolunteerAccount.ROLE);
        if (geRoleResult.IsFailure)
        {
            _logger.LogError("Не найдена роль волонтёра");
            return;
        }

        Role role = geRoleResult.Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, role, avatar: null, id: command.UserId).Value;
        List<Requisites> requisites = command.Requisites.Select(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod).Value).ToList();
        List<Certificate> certificates = command.Certificates.Select(r => Certificate.Create(r.Name, r.Value).Value).ToList();
        Date startVolunteeringDate = Date.Create(command.StartVolunteeringDate).Value;

        VolunteerAccount volunteer = VolunteerAccount.Create(
            user,
            startVolunteeringDate,
            requisites,
            certificates).Value;

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.AddUser(user, CancellationToken.None);
        await _repository.AddVolunteer(volunteer, CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
