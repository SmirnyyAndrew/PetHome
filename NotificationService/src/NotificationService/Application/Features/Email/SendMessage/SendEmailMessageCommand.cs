using NotificationService.Domain;

namespace NotificationService.Application.Features.Email.SendMessage;

public record SendEmailMessageCommand(
    Emails SenderEmailType,
    string RecipientEmail,
    string Subject,
    string Body);