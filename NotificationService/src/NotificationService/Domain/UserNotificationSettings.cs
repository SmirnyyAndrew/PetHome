using NotificationService.Core.VO;

namespace NotificationService.Domain;

public class UserNotificationSettings
{
    public Guid UserId { get; set; }
    public bool? IsEmailSend { get; set; } = false; 
    public TelegramSettings? TelegramSettings { get; set; }
    public bool? IsWebSend { get; set; } = false;

    private UserNotificationSettings() { }
    public UserNotificationSettings(
        Guid userId,
        bool? isEmailSend,
        TelegramSettings? telegramSettings, 
        bool? isWebSend)
    {
        UserId = userId;
        IsEmailSend = isEmailSend;
        TelegramSettings = telegramSettings; 
        IsWebSend = isWebSend;
    }
}
