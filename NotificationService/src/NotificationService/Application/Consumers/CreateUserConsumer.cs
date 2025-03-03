using MassTransit;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using PetHome.Accounts.Contracts.Messaging.UserManagment;

namespace NotificationService.Application.Consumers;

public class CreateUserConsumer(IConfiguration configuration)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        EmailManager emailManager = YandexEmailManager.Build(configuration);
        emailManager.SendMessage(
            command.Email,
            ConfirmationEmailMessage.Subject(),
            ConfirmationEmailMessage.Body(command.UserName, command.EmailConfirmationLink),
            ConfirmationEmailMessage.Styles());
        return;
    }
}
