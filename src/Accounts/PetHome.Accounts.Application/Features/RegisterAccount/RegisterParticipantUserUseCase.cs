using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Aggregates.User.Accounts;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Framework.Database;
namespace PetHome.Accounts.Application.Features.RegisterAccount;
public class RegisterParticipantUserUseCase
    : ICommandHandler<RegisterParticipantUserCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterParticipantUserCommand> _validator;
    private readonly ILogger<RegisterParticipantUserUseCase> _logger;

    public RegisterParticipantUserUseCase(
        IAuthenticationRepository repository,
        UserManager<User> userManager,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IValidator<RegisterParticipantUserCommand> validator,
        ILogger<RegisterParticipantUserUseCase> logger)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }


    public async Task<UnitResult<ErrorList>> Execute(
        RegisterParticipantUserCommand command,
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

        Role role = _repository.GetRole(ParticipantAccount.ROLE).Result.Value;
        UserName userName = UserName.Create(command.Name).Value;
        User user = User.Create(email, userName, role).Value;

        ParticipantAccount participant = ParticipantAccount.Create(user).Value;
        await _repository.AddParticipant(participant, ct);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded is false)
            return result.Errors.ToErrorList();

        await _unitOfWork.SaveChages(ct);
        transaction.Commit();

        _logger.LogInformation("Patrisipant-user с id = {0} добавлен", user.Id);
        return Result.Success<ErrorList>();
    }
}
