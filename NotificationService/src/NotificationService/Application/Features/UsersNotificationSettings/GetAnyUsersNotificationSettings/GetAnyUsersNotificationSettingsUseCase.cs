using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetAnyUsersNotificationSettings;

public class GetAnyUsersNotificationSettingsUseCase(NotificationRepository repository)
    : IQueryHandler<IReadOnlyList<UserNotificationSettings>>
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Execute(CancellationToken ct)
    {
        return await repository.GetAnySending(ct);
    }
}
