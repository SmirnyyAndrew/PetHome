using NotificationService.Application.Dto;
using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;

public record UpdateUserNotificationSettingsCommand(
    Guid UserId, SendingNotificationSettingsDto NewNotificationSettings) : ICommand;