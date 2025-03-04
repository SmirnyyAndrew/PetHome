using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.CreateUser;
public class CreateUserUseCase
    : ICommandHandler<UserId, CreateUserCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public CreateUserUseCase(
        IAuthenticationRepository repository,
        IValidator<CreateUserCommand> validator,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<UserId, ErrorList>> Execute(CreateUserCommand command, CancellationToken ct)
    {
        var validationResult = _validator.Validate(command);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;

        var geRoleResult = await _repository.GetRole(command.RoleId);
        if (geRoleResult.IsFailure)
            return geRoleResult.Error.ToErrorList();

        Role role = geRoleResult.Value;
        User user = User.Create(email, userName, role).Value;
        UserId userId = UserId.Create(user.Id).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        await _unitOfWork.SaveChanges(ct);

        CreatedUserEvent createdUserEvent = new CreatedUserEvent(
            user.Id,
            user.Email,
            user.UserName,
            user.Role?.Name ?? string.Empty);
        await _publisher.Publish(createdUserEvent, ct);
        transaction.Commit();
        transaction.Commit();

        return userId;
    }
}
