using NotificationService.Application.Dto;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;

public record UpdateUserNotificationSettingsCommand(
    Guid UserId, SendingNotificationSettingsDto NewNotificationSettings) : ICommand;