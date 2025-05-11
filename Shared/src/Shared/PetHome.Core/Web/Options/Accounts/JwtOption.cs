namespace PetHome.Core.Web.Options.Accounts;
public class JwtOption
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpiredMinutes { get; init; }
    public const string SECTION_NAME = "JWT";
}
