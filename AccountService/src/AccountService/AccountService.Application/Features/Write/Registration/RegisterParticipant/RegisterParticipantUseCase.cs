using AccountService.Application.Database.Repositories;
using AccountService.Application.Features.Write.Registration.RegisterUser;
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

namespace AccountService.Application.Features.Write.Registration.RegisterParticipant;
public class RegisterParticipantUseCase
    : ICommandHandler<RegisterParticipantCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public RegisterParticipantUseCase(
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
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterParticipantCommand command, CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        User user = getUserResult.Value;
        ParticipantAccount participant = ParticipantAccount.Create(user).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddParticipant(participant, ct);
        await _unitOfWork.SaveChanges(ct);

        CreatedParticipantEvent createdParticipantEvent = new CreatedParticipantEvent(
            user.Id, user.Email, user.UserName);
        await _publisher.Publish(createdParticipantEvent);
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
