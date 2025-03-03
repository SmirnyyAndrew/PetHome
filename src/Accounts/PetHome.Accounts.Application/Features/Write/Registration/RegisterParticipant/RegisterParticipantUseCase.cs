using CSharpFunctionalExtensions;
using MassTransit;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterUser;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterParticipant;
public class RegisterParticipantUseCase
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IPublishEndpoint _publisher;
    public RegisterParticipantUseCase(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterUserCommand command, CancellationToken ct)
    {
        CreatedParticipantEvent createdParticipantEvent = new CreatedParticipantEvent(
            Guid.NewGuid(),
            command.Email,
            command.UserName);
        await _publisher.Publish(createdParticipantEvent);

        return Result.Success<ErrorList>();
    }
}
