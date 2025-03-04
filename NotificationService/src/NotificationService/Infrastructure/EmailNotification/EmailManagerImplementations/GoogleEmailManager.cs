using NotificationService.Core.Options;

namespace NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

public class GoogleEmailManager : IEmailManager
{
    private static readonly string host = "smtp.gmail.com";
    private static readonly int port = 587;

    public static EmailManager Build(IConfiguration configuration)
    {
        var googleOption = configuration.GetSection(EmailOption.GOOGLE).Get<EmailOption>();
        string senderEmail = googleOption.Email;
        string senderPassword = googleOption.Password;

        return EmailManager.Build(senderEmail, senderPassword, host, port);
    }
}
