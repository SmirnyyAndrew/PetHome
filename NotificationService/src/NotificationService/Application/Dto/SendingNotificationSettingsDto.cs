using NotificationService.Core.Dto;

namespace NotificationService.Application.Dto;

public record SendingNotificationSettingsDto(
    bool? IsEmailSend,  
    TelegramSettingsDto? TelegramSettings,
    bool? IsWebSend);
