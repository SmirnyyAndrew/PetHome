using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;

public class GetUserNotificationSettingsUseCase(NotificationRepository repository)
{
    public async Task<UserNotificationSettings?> Execute(Guid userId, CancellationToken ct)
    {
        return await repository.Get(userId, ct);
    }
}
