using CSharpFunctionalExtensions;
using NotificationService.Domain;
using NotificationService.Infrastructure.EmailNotification;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.Email.SendMessage;

public class SendEmailMessageUseCase(IConfiguration configuration)
    : ICommandHandler<SendEmailMessageCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(SendEmailMessageCommand command, CancellationToken ct)
    {
        EmailManager emailManager = (command.SenderEmailType) switch
        {
            Emails.Yandex => YandexEmailManager.Build(configuration),
            Emails.Google => GoogleEmailManager.Build(configuration),
            _ => YandexEmailManager.Build(configuration)
        };

        emailManager.SendMessage(
            command.RecipientEmail,
            command.Subject,
            command.Body);

        return Result.Success<ErrorList>();
    } 
}
