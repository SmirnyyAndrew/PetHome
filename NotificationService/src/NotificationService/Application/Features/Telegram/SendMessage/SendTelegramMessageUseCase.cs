using NotificationService.Infrastructure.TelegramNotification;

namespace NotificationService.Application.Features.Telegram.SendMessage;

public class SendTelegramMessageUseCase
{
    private readonly TelegramManager _telegramManager;

    public SendTelegramMessageUseCase(TelegramManager telegramManager)
    {
        _telegramManager = telegramManager; 
    }

    public async Task Execute(SendTelegramMessageCommand command, CancellationToken ct)
    { 
        await _telegramManager.SendMessage(command.UserId, command.Message);
        return;
    }
}
