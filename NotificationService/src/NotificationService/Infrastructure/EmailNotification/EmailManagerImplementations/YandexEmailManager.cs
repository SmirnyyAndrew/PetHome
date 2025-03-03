using NotificationService.Core.Options;

namespace NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

public static class YandexEmailManager
{
    private static readonly string host = "smtp.yandex.ru";
    private static readonly int port = 587;
    public static EmailManager Build(IConfiguration configuration)
    {
        var yandexOption = configuration.GetSection(EmailOption.YANDEX).Get<EmailOption>();
        string senderEmail = yandexOption.Email;
        string senderPassword = yandexOption.Password;

        return EmailManager.Build(senderEmail, senderPassword, host, port);
    }
}