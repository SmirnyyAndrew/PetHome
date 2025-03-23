using CSharpFunctionalExtensions;
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using NotificationService.Infrastructure.TelegramNotification;
using PetHome.Accounts.Contracts.HttpCommunication;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

public class SendMessageEverywhereUseCase
    : ICommandHandler<SendMessageEverywhereCommand>
{
    private readonly NotificationRepository _repository;
    private readonly TelegramManager _telegramManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration; 
    private readonly AccountHttpClient _httpClient;

    public SendMessageEverywhereUseCase(
        NotificationRepository repository,
        TelegramManager telegramManager,
        IConfiguration configuration, 
        AccountHttpClient httpClient,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _telegramManager = telegramManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
    }

    public async Task<UnitResult<ErrorList>> Execute(SendMessageEverywhereCommand command, CancellationToken ct)
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
            string? email = await _httpClient.GetUserEmailByUserId(command.UserId, ct); 
            EmailManager emailManager = YandexEmailManager.Build(_configuration);
            emailManager.SendMessage(email, command.Subject, command.Body);
        }

        if (userNotificationSettings?.TelegramSettings != null)
        {
            await _telegramManager.SendMessage(command.UserId, command.Body);
        }
        else if (command.TelegramUserId != null)
        {
            await _telegramManager.StartRegisterChatId(command.UserId, command.TelegramUserId);
            await _telegramManager.SendMessage(command.UserId, command.Body);
        }

        return Result.Success<ErrorList>();
    }
}
