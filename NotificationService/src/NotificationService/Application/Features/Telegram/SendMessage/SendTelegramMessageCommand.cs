namespace NotificationService.Application.Features.Telegram.SendMessage;
public record SendTelegramMessageCommand( 
    Guid UserId,
    string Message);