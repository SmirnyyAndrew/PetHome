using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.Telegram.SendMessage;
public record SendTelegramMessageCommand(
    Guid UserId,
    string Message) : ICommand;