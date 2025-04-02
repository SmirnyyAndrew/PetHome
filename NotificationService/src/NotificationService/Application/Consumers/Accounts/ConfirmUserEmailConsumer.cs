using AccountService.Contracts.Messaging.UserManagment;
using MassTransit;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

namespace NotificationService.Application.Consumers.Accounts;

public class ConfirmUserEmailConsumer(IConfiguration configuration)
    : IConsumer<ConfirmedUserEmailEvent>
{
    public async Task Consume(ConsumeContext<ConfirmedUserEmailEvent> context)
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
