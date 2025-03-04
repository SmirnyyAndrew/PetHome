namespace NotificationService.Application.Dto;

public record SendingNotificationSettings(
    bool? IsEmailSend, 
    bool? IsTelegramSend, 
    long? TelegramChatId,
    bool? IsWebSend);
