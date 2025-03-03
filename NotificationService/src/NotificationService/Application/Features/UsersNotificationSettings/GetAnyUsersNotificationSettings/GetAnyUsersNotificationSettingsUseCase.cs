using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetAnyUsersNotificationSettings;

public class GetAnyUsersNotificationSettingsUseCase(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetAnySending(ct);
    }
}
