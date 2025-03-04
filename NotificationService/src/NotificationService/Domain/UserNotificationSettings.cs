namespace NotificationService.Domain;

public class UserNotificationSettings
{
    public Guid UserId { get; set; }
    public bool? IsEmailSend { get; set; } = false;
    public bool? IsTelegramSend { get; set; } = false;
    public bool? IsWebSend { get; set; } = false;

}
