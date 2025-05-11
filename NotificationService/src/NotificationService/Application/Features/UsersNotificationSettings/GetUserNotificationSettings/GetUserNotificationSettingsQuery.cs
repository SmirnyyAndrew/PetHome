using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;

public record GetUserNotificationSettingsQuery(Guid UserId) : IQuery;
