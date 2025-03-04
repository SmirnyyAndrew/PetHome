namespace NotificationService.Domain;

public class UserNotificationSettings
{
    public Guid UserId { get; set; }
    public bool? IsEmailSend { get; set; } = false;
    public bool? IsTelegramSend { get; set; } = false;
    public long? TelegramChatId { get; set; }
    public bool? IsWebSend { get; set; } = false;

    public UserNotificationSettings(
        Guid userId, 
        bool? isEmailSend, 
        bool? isTelegramSend, 
        long? telegramChatId,
        bool? isWebSend)
    {
        UserId = userId;
        IsEmailSend = isEmailSend;
        IsTelegramSend = isTelegramSend;
        TelegramChatId = telegramChatId;
        IsWebSend = isWebSend;
    }
}
