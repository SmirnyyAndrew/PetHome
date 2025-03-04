using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUsersTelegramSendings;

public class GetUsersTelegramSendingsUseCase(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetTelegramSendings(ct);
    }
}
