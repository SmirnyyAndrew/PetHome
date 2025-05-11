namespace PetHome.Core.Web.Options.Accounts;
public class AdminOption
{
    public const string SECTION_NAME = "ADMIN";
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
