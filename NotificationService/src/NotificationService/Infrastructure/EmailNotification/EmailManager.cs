using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NotificationService.Core.EmailMessages;

namespace NotificationService.Infrastructure.EmailNotification;

public class EmailManager
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _senderEmail = "email@email";
    private readonly string _senderPassword = "password";

    private EmailManager(string senderEmail, string senderPassword, string host, int port)
    {
        _host = host;
        _port = port;
        _senderEmail = senderEmail;
        _senderPassword = senderPassword;
    }

    public void SendMessage(
        string recipientEmail,
        string subject,
        string body,
        string styles = "",
        string senderEmailName = "PetHome")
    {

        string emailMessage = EmailMessageConstructor.Build(styles, body);

        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderEmailName, _senderEmail));
        message.To.Add(new MailboxAddress(string.Empty, recipientEmail));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = emailMessage
        };

        using (SmtpClient client = new SmtpClient())
        {
            client.Connect(_host, _port, SecureSocketOptions.StartTls);
            client.Authenticate(_senderEmail, _senderPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }

    public static EmailManager Build(
        string senderEmail,
        string senderPassword,
        string host,
        int port)
    {
        EmailManager manager = new EmailManager(senderEmail, senderPassword, host, port);
        return manager;
    }
}
