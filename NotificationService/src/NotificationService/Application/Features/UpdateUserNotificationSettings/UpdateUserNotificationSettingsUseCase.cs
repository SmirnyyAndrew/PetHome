using NotificationService.Application.Dto;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UpdateUserNotificationSettings;

public class UpdateUserNotificationSettingsUseCase(NotificationRepository repository)
{
    public async Task Execute(
        Guid userId, SendingNotificationSettings newNotificationSettings, CancellationToken ct)
    {
        await repository.Update(userId, newNotificationSettings, ct);
    }
}
