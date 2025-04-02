using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.UsersNotificationSettings.ResetUserNotificationSettings;

public record ResetUserNotificationSettingsCommand(Guid UserId) : ICommand;