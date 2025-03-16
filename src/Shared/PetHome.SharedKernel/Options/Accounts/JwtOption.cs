namespace PetHome.SharedKernel.Options.Accounts;
public class JwtOption
{
    public string Issuer { get; init; } = String.Empty;
    public string Audience { get; init; } = String.Empty;
    public string Key { get; init; } = String.Empty;
    public int ExpiredMinutes { get; init; }
    public const string SECTION_NAME = "JWT";
}
