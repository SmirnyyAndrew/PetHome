using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUsersEmailSendings;

public class GetUsersEmailSendingsUseCase(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetEmailSendings(ct);
    }
}
