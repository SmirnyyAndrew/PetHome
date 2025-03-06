using NotificationService.Application.Dto;
using NotificationService.Infrastructure.Database;

namespace NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;

public class UpdateUserNotificationSettingsUseCase(
    NotificationRepository repository, UnitOfWork unitOfWork)
{
    public async Task Execute(
        Guid userId, SendingNotificationSettingsDto newNotificationSettings, CancellationToken ct)
    {
        await repository.Update(userId, newNotificationSettings, ct);
        await unitOfWork.SaveChanges(ct);
    }
}
