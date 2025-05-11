using CSharpFunctionalExtensions; 
using NotificationService.Infrastructure.TelegramNotification;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace NotificationService.Application.Features.Telegram.SendMessage;

public class SendTelegramMessageUseCase
    : ICommandHandler<SendTelegramMessageCommand>
{
    private readonly TelegramManager _telegramManager;

    public SendTelegramMessageUseCase(TelegramManager telegramManager)
    {
        _telegramManager = telegramManager; 
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SendTelegramMessageCommand command, CancellationToken ct)
    { 
        await _telegramManager.SendMessage(command.UserId, command.Message);

        return Result.Success<ErrorList>();
    } 
}
