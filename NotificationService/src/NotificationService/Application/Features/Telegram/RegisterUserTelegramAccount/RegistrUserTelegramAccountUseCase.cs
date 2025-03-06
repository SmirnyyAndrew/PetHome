using NotificationService.Application.Features.Telegram.SendMessage;
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.TelegramNotification;

namespace NotificationService.Application.Features.Telegram.RegisterUserTelegramAccount;

public class RegisterUserTelegramAccountUseCase
{
    private readonly TelegramManager _telegramManager;
    private readonly NotificationRepository _repository;

    public RegisterUserTelegramAccountUseCase(
        TelegramManager telegramManager,
        NotificationRepository repository)
    {
        _telegramManager = telegramManager;
        _repository = repository;
    }

    public async Task Execute(RegisterUserTelegramAccountCommand command, CancellationToken ct)
    { 
        await _telegramManager.StartRegisterChatId(command.UserId,command.UserTelegramId);
        return;
    }
}
