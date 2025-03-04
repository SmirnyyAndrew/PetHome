namespace NotificationService.Infrastructure.EmailNotification;

public interface IEmailManager
{
    public static abstract EmailManager Build(IConfiguration configuration);
}
