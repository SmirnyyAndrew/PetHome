using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;

public record GetUserNotificationSettingsQuery(Guid UserId) : IQuery;
