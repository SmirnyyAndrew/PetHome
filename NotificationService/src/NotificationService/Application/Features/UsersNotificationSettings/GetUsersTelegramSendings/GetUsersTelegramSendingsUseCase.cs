using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUsersTelegramSendings;

public class GetUsersTelegramSendingsUseCase(NotificationRepository repository)
    : IQueryHandler<IReadOnlyList<UserNotificationSettings>>
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetTelegramSendings(ct);
    }
}
