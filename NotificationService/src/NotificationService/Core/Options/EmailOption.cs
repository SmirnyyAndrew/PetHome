namespace NotificationService.Core.Options;

public class EmailOption
{
    public const string YANDEX = "YANDEX";
    public const string GOOGLE = "GOOGLE";
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; 
}
