using Microsoft.AspNetCore.Identity;

namespace AccountService.Infrastructure.DI.Auth;
public static class AuthOption
{
    public static IdentityOptions GetAuthenticationOptions(this IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;

        return options;
    }
}
