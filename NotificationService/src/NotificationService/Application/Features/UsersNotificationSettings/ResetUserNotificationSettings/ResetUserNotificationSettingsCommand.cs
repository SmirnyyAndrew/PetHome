using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.UsersNotificationSettings.ResetUserNotificationSettings;

public record ResetUserNotificationSettingsCommand(Guid UserId) : ICommand;