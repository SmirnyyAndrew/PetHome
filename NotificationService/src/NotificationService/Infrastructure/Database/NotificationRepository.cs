using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Dto;
using NotificationService.Domain;

namespace NotificationService.Infrastructure.Database;

public class NotificationRepository(NotificationDbContext dbContext)
{  
    public async Task<UserNotificationSettings?> Get(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications 
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetAnySending(CancellationToken ct)
    {
        var getResult = await dbContext.Notifications 
            .Where(n => n.IsEmailSend == true || n.IsWebSend == true || n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetEmailSendings(CancellationToken ct)
    {
        var getResult = await dbContext.Notifications 
            .Where(n => n.IsEmailSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetTelegramSendings(CancellationToken ct)
    {
        var getResult = await dbContext.Notifications 
            .Where(n => n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetWebSendings(CancellationToken ct)
    {
        var getResult = await dbContext.Notifications 
            .Where(n => n.IsWebSend == true)
            .ToListAsync(ct);
        return getResult;
    }
     
    public async Task<long?> GetTelegramChatId(Guid userId, CancellationToken ct)
    {
        long? chatId = dbContext.Notifications 
            .FirstOrDefaultAsync(n => n.UserId == userId)
            .Result?.TelegramChatId;
        return chatId;
    }

    public async Task Update(
        Guid userId,
        SendingNotificationSettings newNotificationSettings,
        CancellationToken ct)
    {
        var userNotification = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        if (userNotification is null)
        {
            UserNotificationSettings userNotificationSettings = new UserNotificationSettings(
                userId,
                newNotificationSettings.IsEmailSend,
                newNotificationSettings.IsTelegramSend,
                newNotificationSettings.TelegramChatId,
                newNotificationSettings.IsWebSend);
            await dbContext.Notifications.AddAsync(userNotificationSettings, ct);

            return;
        }

        userNotification.IsEmailSend = newNotificationSettings.IsEmailSend
            ?? userNotification.IsEmailSend;

        userNotification.IsTelegramSend = newNotificationSettings.IsTelegramSend
            ?? userNotification.IsTelegramSend;

        userNotification.TelegramChatId = newNotificationSettings.TelegramChatId
            ?? userNotification.TelegramChatId;

        userNotification.IsWebSend = newNotificationSettings.IsWebSend
            ?? userNotification.IsWebSend;
    }

    public async Task Reset(Guid userId, CancellationToken ct)
    {
        UserNotificationSettings? oldNotificationSettings = await Get(userId, ct);

        SendingNotificationSettings newNotificationSettings;
        if (oldNotificationSettings is null)
            newNotificationSettings = new SendingNotificationSettings(false, false, null, false);
        else
        {
            newNotificationSettings = new SendingNotificationSettings(
                false, false, oldNotificationSettings?.TelegramChatId, false);
        }

        await Update(userId, newNotificationSettings, ct);
    }

    public async Task AddTelegramChatId(Guid userId, long telegramChatId, CancellationToken ct)
    {
        UserNotificationSettings? notifcationSettings = await Get(userId, ct);

        SendingNotificationSettings newNotificationSettings = new(
            notifcationSettings?.IsEmailSend,
            IsTelegramSend: true,
            telegramChatId,
            notifcationSettings?.IsWebSend);

        await Update(userId, newNotificationSettings, ct);
    } 
}
