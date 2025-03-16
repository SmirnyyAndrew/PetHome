using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterUser;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterParticipant;
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
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
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
