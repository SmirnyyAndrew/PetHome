using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Dto;
using NotificationService.Core.Dto;
using NotificationService.Core.VO;
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
            .Where(n => n.IsEmailSend == true || n.IsWebSend == true || n.TelegramSettings != null)
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
            .Where(n => n.TelegramSettings != null)
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
     
    public async Task<TelegramSettings?> GetTelegramSettings(Guid userId, CancellationToken ct)
    {
        var notificationSettings = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId);
        if(notificationSettings is null)
            return null;

        return notificationSettings.TelegramSettings;
    }

    public async Task Update(
        Guid userId,
        SendingNotificationSettingsDto newNotificationSettings,
        CancellationToken ct)
    {
        var userNotification = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);

        var telegramUserId = newNotificationSettings?.TelegramSettings?.TelegramUserId;
        var telegramChatId = newNotificationSettings?.TelegramSettings?.TelegramChatId;
        TelegramSettings? telegramSettings = null;
        if (telegramChatId != null && telegramUserId != null)
            telegramSettings = new TelegramSettings(telegramUserId, (long)telegramChatId!);
         
        if (userNotification is null)
        {  
            UserNotificationSettings userNotificationSettings = new UserNotificationSettings(
                userId,
                newNotificationSettings?.IsEmailSend,
                telegramSettings,
                newNotificationSettings?.IsWebSend);
            await dbContext.Notifications.AddAsync(userNotificationSettings, ct);

            return;
        }

        userNotification.IsEmailSend = newNotificationSettings?.IsEmailSend
            ?? userNotification.IsEmailSend;

        userNotification.TelegramSettings = telegramSettings; 

        userNotification.IsWebSend = newNotificationSettings?.IsWebSend
            ?? userNotification.IsWebSend;
    }

    public async Task Reset(Guid userId, CancellationToken ct)
    {
        UserNotificationSettings? oldNotificationSettings = await Get(userId, ct);

        SendingNotificationSettingsDto newNotificationSettings 
            = new(IsEmailSend: false, TelegramSettings: null, IsWebSend: false);

        await Update(userId, newNotificationSettings, ct);
    }

    public async Task AddTelegramSettings(
        Guid userId, long telegramChatId, string telegramUserId, CancellationToken ct)
    {
        UserNotificationSettings? notifcationSettings = await Get(userId, ct);

        TelegramSettingsDto telegramSettingsDto = 
            new TelegramSettingsDto(telegramUserId, telegramChatId);

        SendingNotificationSettingsDto newNotificationSettings = new(
            notifcationSettings?.IsEmailSend,
            telegramSettingsDto,
            notifcationSettings?.IsWebSend);

        await Update(userId, newNotificationSettings, ct);
    } 
}
