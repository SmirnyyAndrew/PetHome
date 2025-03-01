using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
using NotificationService.Features.Dto;

namespace NotificationService.Infrastructure.Database.Write;

public class NotificationRepository(NotificationWriteDbContext dbContext)
{
    public async Task<UserNotificationSettings?> Get(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetAnySending(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsEmailSend == true || n.IsWebSend == true || n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetEmailSending(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsEmailSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetTelegramSending(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetWebSending(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsWebSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task Update(
        Guid userId,
        SendingNotificationSettings newNotificationSettings,
        CancellationToken ct)
    {
        var userNotification = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        if (userNotification is null)
            return;

        userNotification.IsEmailSend = newNotificationSettings.IsEmailSend
            ?? userNotification.IsEmailSend;

        userNotification.IsTelegramSend = newNotificationSettings.IsTelegramSend
            ?? userNotification.IsTelegramSend;

        userNotification.IsWebSend = newNotificationSettings.IsWebSend
            ?? userNotification.IsWebSend;
    }

    public async Task Reset(Guid userId, CancellationToken ct)
    {
        SendingNotificationSettings newNotificationSettings =
            new SendingNotificationSettings(false, false, false);

        await Update(userId, newNotificationSettings, ct);
    }
}
