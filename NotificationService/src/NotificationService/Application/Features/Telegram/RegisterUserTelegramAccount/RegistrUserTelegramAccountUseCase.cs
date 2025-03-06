using CSharpFunctionalExtensions; 
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.TelegramNotification;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.Telegram.RegisterUserTelegramAccount;

public class RegisterUserTelegramAccountUseCase
    : ICommandHandler<RegisterUserTelegramAccountCommand>
{
    private readonly TelegramManager _telegramManager;
    private readonly NotificationRepository _repository;

    public RegisterUserTelegramAccountUseCase(
        TelegramManager telegramManager,
        NotificationRepository repository)
    {
        _telegramManager = telegramManager;
        _repository = repository;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RegisterUserTelegramAccountCommand command, CancellationToken ct)
    { 
        await _telegramManager.StartRegisterChatId(command.UserId,command.UserTelegramId);
       
        return Result.Success<ErrorList>();
    } 
}
