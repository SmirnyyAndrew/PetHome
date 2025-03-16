using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.Telegram.RegisterUserTelegramAccount;

public record RegisterUserTelegramAccountCommand(
    Guid UserId, string UserTelegramId) : ICommand;
