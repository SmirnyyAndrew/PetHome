using NotificationService.Domain;
using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.Application.Features.Email.SendMessage;

public record SendEmailMessageCommand(
    Emails SenderEmailType,
    string RecipientEmail,
    string Subject,
    string Body) : ICommand;