namespace PetHome.SharedKernel.Options.Accounts;
public class RefreshTokenOption
{
    public int ExpiredDays { get; init; } = 7;
    public const string SECTION_NAME = "RefreshToken";
}
