namespace NotificationService.Application.Features.Telegram.RegisterUserTelegramAccount;

public record RegisterUserTelegramAccountCommand(
    Guid UserId, string UserTelegramId);
