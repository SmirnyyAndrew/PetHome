using MassTransit;
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
            "Регистрация", 
            $"Добро пожаловать, {command.UserName}! Для подтверждения почты перейдите по ссылке {command.UserId}.com"); 
        return;
    }
}
