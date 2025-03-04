using MassTransit;
using NotificationService.Infrastructure.Database;
using PetHome.Accounts.Contracts.Messaging.UserManagment;

namespace NotificationService.Application.Consumers;

public class CreateUserConsumer
    (NotificationRepository repository) : IConsumer<CreatedUserEvent>
{
    public async Task Consume(ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message; 
        await repository.Reset(command.UserId, CancellationToken.None);
        return;
    }
}
