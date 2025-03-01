namespace PetHome.SharedKernel.Options.Volunteers;
public class RabbitMqOption
{
    public const string SECTION_NAME = "RabbitMq";
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
