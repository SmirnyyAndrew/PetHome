using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetHome.SharedKernel.Options.Accounts;
using System.Text;

namespace AccountService.Infrastructure.DI.Auth;
public static class TokenValidationManager
{
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        JwtOption _options = GetJwtOptions(configuration);

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
            ClockSkew = TimeSpan.FromMinutes(100)
        };

        return tokenValidationParameters;
    } 

    public static JwtOption GetJwtOptions(IConfiguration configuration)
    {
        return configuration.GetSection(JwtOption.SECTION_NAME).Get<JwtOption>()
             ?? throw new ApplicationException("Missing JWT configuration");
    }
}
