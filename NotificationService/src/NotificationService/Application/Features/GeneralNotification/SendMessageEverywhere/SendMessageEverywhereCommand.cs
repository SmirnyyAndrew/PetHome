using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

public record SendMessageEverywhereCommand(
    Guid UserId, 
    string? TelegramUserId, 
    string Body, 
    string? Subject = null) : ICommand;
