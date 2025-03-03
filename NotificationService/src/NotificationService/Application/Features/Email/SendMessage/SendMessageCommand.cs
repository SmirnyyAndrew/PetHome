using NotificationService.Domain;

namespace NotificationService.Application.Features.Email.SendMessage;

public record SendMessageCommand(
    Emails SenderEmailType,
    string RecipientEmail,
    string Subject,
    string Body);