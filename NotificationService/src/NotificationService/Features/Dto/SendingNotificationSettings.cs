namespace NotificationService.Features.Dto;

public record SendingNotificationSettings(bool? IsEmailSend, bool? IsTelegramSend, bool? IsWebSend);
