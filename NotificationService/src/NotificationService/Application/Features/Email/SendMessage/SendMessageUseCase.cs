using NotificationService.Domain;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

namespace NotificationService.Application.Features.Email.SendMessage;

public class SendMessageUseCase(IConfiguration configuration)
{
    public async Task Execute(SendMessageCommand command, CancellationToken ct)
    {
        EmailManager emailManager = (command.SenderEmailType) switch
        {
            Emails.Yandex => YandexEmailManager.Build(configuration),
            Emails.Google => GoogleEmailManager.Build(configuration),
            _ => YandexEmailManager.Build(configuration)
        };

        emailManager.SendMessage(
            command.RecipientEmail,
            command.Subject,
            command.Body);
    }
}
