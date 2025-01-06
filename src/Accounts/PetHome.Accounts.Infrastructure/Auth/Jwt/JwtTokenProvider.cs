using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetHome.Accounts.Application;
using PetHome.Accounts.Domain.Aggregates.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetHome.Accounts.Infrastructure.Auth.Jwt;
public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public JwtTokenProvider(IConfiguration configuration)
    {
        _options = configuration.GetSection(JwtOptions.NAME).Get<JwtOptions>()
            ?? throw new ApplicationException("Missing JWT configuration"); ;
    }

    public async Task<string> GenerateToken(User user, CancellationToken ct)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiredMinute));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return "Bearer " + tokenString;
    }
}
