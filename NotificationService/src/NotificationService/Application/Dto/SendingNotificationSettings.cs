namespace NotificationService.Application.Dto;

public record SendingNotificationSettings(bool? IsEmailSend, bool? IsTelegramSend, bool? IsWebSend);
