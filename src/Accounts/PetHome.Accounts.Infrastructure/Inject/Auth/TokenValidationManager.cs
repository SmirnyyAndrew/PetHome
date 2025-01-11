using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetHome.Accounts.Infrastructure.Auth.Jwt;
using System.Text;

namespace PetHome.Accounts.Infrastructure.Inject.Auth;
public static class TokenValidationManager
{
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        JwtOptions _options = GetJwtOptions(configuration);

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

    public static JwtOptions GetJwtOptions(IConfiguration configuration)
    {
        return configuration.GetSection(JwtOptions.NAME).Get<JwtOptions>()
             ?? throw new ApplicationException("Missing JWT configuration");
    }
}
