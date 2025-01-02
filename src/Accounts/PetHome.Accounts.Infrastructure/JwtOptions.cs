namespace PetHome.Accounts.Infrastructure;
public class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
    public int ExpiredMinute { get; init; }
    public const string NAME = "JWT";
}
