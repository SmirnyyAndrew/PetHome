using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetHome.Accounts.Application;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Tokens.RefreshToken;
using PetHome.Accounts.Infrastructure.Inject.Auth;
using PetHome.Core.Response.ErrorManagment;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetHome.Accounts.Infrastructure.Auth.Jwt;
public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;
    private readonly IConfiguration _configuration;

    public TokenProvider(IConfiguration configuration)
    {
        _options = TokenValidationManager.GetJwtOptions(configuration);
        _configuration = configuration;
    }

    public Result<RefreshSession, Error> GenerateRefreshToken(
        User user,
        string accessToken)
    { 
        var validations = TokenValidationManager.GetTokenValidationParameters(_configuration);
        var handler = new JwtSecurityTokenHandler();
        var claims = handler.ValidateToken(accessToken, validations, out var tokenSecure);

        var jtiClaim = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        var userIdClaim = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        bool isJtiParsed = Guid.TryParse(jtiClaim?.Value, out Guid jti);
        bool isUserIdParsed = Guid.TryParse(jtiClaim?.Value, out Guid userId);

        if (isJtiParsed && isUserIdParsed)
        {
            bool isValidUser = userId == user.Id;
            if (isValidUser)
            {
                RefreshSession refreshSession = new RefreshSession()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiredIn = DateTime.UtcNow.AddDays(10),
                    User = user,
                    RefreshToken = Guid.NewGuid(),
                    JTI = jti
                };
                return refreshSession;
            }
        }
        return Errors.Validation("Refresh token");
    }

    public string GenerateAccessToken(User user)
    {
        Guid jti = Guid.NewGuid();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email?? string.Empty)
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
        return tokenString;
    }
}
