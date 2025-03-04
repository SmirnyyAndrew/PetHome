using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.TelegramNotification;

namespace NotificationService.Application.Features.Telegram.SendMessage;

public class SendTelegramMessageUseCase
{
    private readonly TelegramManager _telegramManager;
    private readonly NotificationRepository _repository;

    public SendTelegramMessageUseCase(
        TelegramManager telegramManager,
        NotificationRepository repository)
    {
        _telegramManager = telegramManager;
        _repository = repository;
    }

    public async Task Execute(SendTelegramMessageCommand command, CancellationToken ct)
    {
        await _telegramManager.StartRegisterChatId(command.UserId); 
        await _telegramManager.SendMessage(command.UserId, command.Message);
        return;
    }
}
