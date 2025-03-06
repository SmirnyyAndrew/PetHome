using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUsersEmailSendings;

public class GetUsersEmailSendingsUseCase(NotificationRepository repository)
    : IQueryHandler<IReadOnlyList<UserNotificationSettings>>
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetEmailSendings(ct);
    }
}
