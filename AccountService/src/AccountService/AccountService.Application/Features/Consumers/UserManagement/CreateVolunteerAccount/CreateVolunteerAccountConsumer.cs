using AccountService.Application.Database.Repositories;
using AccountService.Contracts.Messaging.UserManagement;
using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Infrastructure.Database;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetHome.SharedKernel.ValueObjects.RolePermission;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Consumers.UserManagement.CreateVolunteerAccount;
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
        [FromKeyedServices(Constants.Database.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
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

        var getRoleResult = await _repository.GetRole(VolunteerAccount.ROLE);
        if (getRoleResult.IsFailure)
        {
            _logger.LogError("Не найдена роль волонтёра");
            return;
        }

        RoleId roleId = RoleId.Create(getRoleResult.Value.Id).Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, roleId, avatar: null, id: command.UserId).Value;
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
