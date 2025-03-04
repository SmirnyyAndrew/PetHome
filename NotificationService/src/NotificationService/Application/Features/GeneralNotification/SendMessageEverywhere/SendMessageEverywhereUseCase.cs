using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using NotificationService.Infrastructure.TelegramNotification;

namespace NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

public class SendMessageEverywhereUseCase
{
    private readonly NotificationRepository _repository;
    private readonly TelegramManager _telegramManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public SendMessageEverywhereUseCase(
        NotificationRepository repository,
        TelegramManager telegramManager,
        IConfiguration configuration,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _telegramManager = telegramManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(SendMessageEverywhereCommand command, CancellationToken ct)
    {
        var userNotificationSettings = await _repository.Get(command.UserId, ct);
        if (userNotificationSettings is null)
        {
            await _repository.Reset(command.UserId, ct);
            var transaction = await _unitOfWork.BeginTransaction(ct);
            await _unitOfWork.SaveChanges(ct);
            transaction.Commit();

            userNotificationSettings = await _repository.Get(command.UserId, ct);
        }

        if (userNotificationSettings?.IsWebSend == true)
        {
            //
        }

        if (userNotificationSettings?.IsEmailSend == true)
        {
            //TODO: добавить контракт в Accounts на получение почты
            string recipientEmail = "smirnay2001@mail.ru";
            EmailManager emailManager = YandexEmailManager.Build(_configuration);
            emailManager.SendMessage(recipientEmail, command.Subject, command.Body);
        }

        if (userNotificationSettings?.IsTelegramSend == true)
        {
            if (userNotificationSettings.TelegramChatId is null)
                await _telegramManager.StartRegisterChatId(command.UserId);

            await _telegramManager.SendMessage(command.UserId, command.Body);
        }
    }
}
