namespace NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

public record SendMessageEverywhereCommand(Guid UserId, string Body, string Subject = null);
