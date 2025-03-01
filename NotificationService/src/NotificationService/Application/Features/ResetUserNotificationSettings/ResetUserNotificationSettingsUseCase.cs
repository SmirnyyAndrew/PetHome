using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.ResetUserNotificationSettings;

public class ResetUserNotificationSettingsUseCase(
    NotificationRepository repository, UnitOfWork unitOfWork)
{
    public async Task Execute(Guid userId, CancellationToken ct)
    {
        await repository.Reset(userId, ct);
        await unitOfWork.SaveChanges(ct);
    }
}
