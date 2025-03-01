using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.GetAnyUsersNotificationSettings;

public class GetAnyUsersNotificationSettingsUseCase(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    { 
        return await repository.GetAnySending(ct);
    }
}
