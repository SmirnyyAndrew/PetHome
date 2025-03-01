using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.GetUserNotificationSettings;

public class GetUserNotificationSettingsUseCase(NotificationRepository repository)
{
    public async Task<UserNotificationSettings?> Execute(Guid userId, CancellationToken ct)
    { 
        return await repository.Get(userId, ct);
    }
}
