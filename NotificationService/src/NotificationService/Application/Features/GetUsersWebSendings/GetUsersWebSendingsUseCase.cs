using NotificationService.Domain;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.GetUsersWebSendings;

public class GetUsersWebSendingsUseCase(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    { 
        return await repository.GetWebSendings(ct);
    }
}
