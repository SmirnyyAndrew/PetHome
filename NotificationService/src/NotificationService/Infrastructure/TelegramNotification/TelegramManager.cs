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
    private long? _chatId;

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
        var dbChatId = await _repository.GetTelegramChatId(userId, CancellationToken.None);
        if (dbChatId is null)
            return;

        await _botClient.SendMessage(dbChatId, message);
        return;
    }

    public async Task StartRegisterChatId(Guid userId)
    {
        UserNotificationSettings? notificationSettings = await _repository.Get(userId, CancellationToken.None);
        if (notificationSettings?.TelegramChatId is not null)
            return;

        _botClient.OnMessage += AddUserChatIdToDB;

        //Ожидание получения номера чата с пользователем
        while (_chatId is null)
            await Task.Delay(100);

        await _repository.AddTelegramChatId(userId, (long)_chatId, CancellationToken.None);
        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();
        return;
    }

    private void StopRegisterChatId() => _botClient.OnMessage -= AddUserChatIdToDB;

    private async Task AddUserChatIdToDB(
        Message message, UpdateType type)
    {
        StopRegisterChatId();
        _chatId = message.Chat.Id;
        return;
    }
}
