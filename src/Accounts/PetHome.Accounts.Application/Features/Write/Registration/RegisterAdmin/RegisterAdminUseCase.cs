using CSharpFunctionalExtensions;
using MassTransit;
using PetHome.Accounts.Application.Features.Write.Registration.RegisterAccount;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterVolunteerAccount;
public class RegisterAdminUseCase
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IPublishEndpoint _publisher;
    public RegisterAdminUseCase(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterUserCommand command, CancellationToken ct)
    {
        CreatedAdminEvent createdAdminEvent = new CreatedAdminEvent(
              Guid.NewGuid(),
              command.Email,
              command.UserName);
        await _publisher.Publish(createdAdminEvent);

        return Result.Success<ErrorList>();
    }
}
