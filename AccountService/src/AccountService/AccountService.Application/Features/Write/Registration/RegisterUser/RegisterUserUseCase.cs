using AccountService.Application.Database.Repositories;
using AccountService.Contracts.Messaging.UserManagement;
using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.RolePermission;
using PetHome.SharedKernel.ValueObjects.User;
namespace AccountService.Application.Features.Write.Registration.RegisterUser;
public class RegisterUserUseCase
    : ICommandHandler<User, RegisterUserCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;
    private readonly IValidator<RegisterUserCommand> _validator;
    private readonly ILogger<RegisterUserUseCase> _logger;

    public RegisterUserUseCase(
        IAuthenticationRepository repository,
        UserManager<User> userManager,
        [FromKeyedServices(Constants.Database.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IPublishEndpoint publisher,
        IValidator<RegisterUserCommand> validator,
        ILogger<RegisterUserUseCase> logger)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _validator = validator;
        _logger = logger;
    }


    public async Task<Result<User, ErrorList>> Execute(
        RegisterUserCommand command,
        CancellationToken ct)
    {
        var validateResult = await _validator.ValidateAsync(command, ct);
        if (validateResult.IsValid is false)
            return validateResult.Errors.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);

        Email email = Email.Create(command.Email).Value;
        var userIsExist = await _repository.GetUserByEmail(email, ct);
        if (userIsExist.IsSuccess)
            return Errors.Conflict($"Email = {command.Email}").ToErrorList();

        var roleResult = await _repository.GetRole(ParticipantAccount.ROLE);
        if (roleResult.IsFailure)
            return roleResult.Error.ToErrorList();

        RoleId roleId = RoleId.Create(roleResult.Value.Id).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, roleId).Value;

        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded is false)
            return result.Errors.ToErrorList();
        await _unitOfWork.SaveChanges(ct);

        //TODO: раскомментировать
        //CreatedUserEvent createdUserEvent = new CreatedUserEvent(
        //    user.Id,
        //    user.Email,
        //    user.UserName,
        //    user.Role?.Name);
        //await _publisher.Publish(createdUserEvent, ct);

        transaction.Commit();

        _logger.LogInformation("User с id = {0} добавлен", user.Id);
        return user;
    }
}
