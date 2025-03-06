using NotificationService.Core.Options;
using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationService.Infrastructure.TelegramNotification;

public class TelegramManager
{
    private readonly TelegramBotClient _botClient;
    private readonly NotificationRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    private long? _telegramChatId;
    private string? _telegramUserId;
    private int? MAX_SECONDS_TO_INITIALIZE = 100;

    public TelegramManager(
        IConfiguration configuration,
        UnitOfWork unitOfWork,
        NotificationRepository repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;

        var telegramOptions = configuration.GetSection(TelegramOption.SECTION_NAME).Get<TelegramOption>();
        _botClient = new TelegramBotClient(telegramOptions!.API);
    }
    public async Task SendMessage(Guid userId, string message)
    {
        var telegramSettings = await _repository.GetTelegramSettings(userId, CancellationToken.None);
        if (telegramSettings is null)
            return;

        await _botClient.SendMessage(telegramSettings.ChatId, message);
        return;
    }

    public async Task StartRegisterChatId(Guid userId, string telegramUserId)
    {
        UserNotificationSettings? notificationSettings = await _repository.Get(userId, CancellationToken.None);
        if (notificationSettings?.TelegramSettings is not null)
            return;

        _telegramUserId = telegramUserId;
        _botClient.OnMessage += AddUserChatIdToDB;

        //Ожидание получения номера чата с пользователем
        int secondsCount = 0;
        while (_telegramChatId is null)
        {
            await Task.Delay(1000);
            secondsCount++;

            if(secondsCount > MAX_SECONDS_TO_INITIALIZE) 
                return;
        }   

        await _repository.AddTelegramSettings(userId, (long)_telegramChatId, _telegramUserId, CancellationToken.None);
        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();
        return;
    }

    private void StopRegisterChatId() => _botClient.OnMessage -= AddUserChatIdToDB;

    private async Task AddUserChatIdToDB(
        Message message, UpdateType type)
    {
        if (message.Chat.Username?.ToLower() != _telegramUserId)
            return;

        StopRegisterChatId();
        _telegramChatId = message.Chat.Id;
    }
}
