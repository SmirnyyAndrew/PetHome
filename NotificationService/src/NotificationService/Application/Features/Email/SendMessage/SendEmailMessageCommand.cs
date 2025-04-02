using NotificationService.Domain;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.Application.Features.Email.SendMessage;

public record SendEmailMessageCommand(
    Emails SenderEmailType,
    string RecipientEmail,
    string Subject,
    string Body) : ICommand;