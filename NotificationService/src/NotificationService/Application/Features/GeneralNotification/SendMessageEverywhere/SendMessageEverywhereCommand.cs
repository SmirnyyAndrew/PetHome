using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

public record SendMessageEverywhereCommand(
    Guid UserId, 
    string? TelegramUserId, 
    string Body, 
    string? Subject = null) : ICommand;
