using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetHome.Accounts.Infrastructure.Auth.Jwt;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Core.Interfaces;
using PetHome.SharedKernel.ValueObjects.AuthAggregates.RolePermission;
using PetHome.SharedKernel.ValueObjects.AuthAggregates.User;
using System.Text;

namespace PetHome.Accounts.Infrastructure.Inject.Auth;
public static class InjectAuthentication
{ 
    public static IServiceCollection ApplyAuthenticationConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        JwtOptions _options = configuration.GetSection(JwtOptions.NAME).Get<JwtOptions>()!;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidIssuer = _options.Issuer,
                     ValidAudience = _options.Audience,
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = false,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                     ClockSkew = TimeSpan.Zero
                 };
             });
        services.AddAuthorization();

        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.AddIdentity<User, Role>(options =>
        {
            options.GetAuthenticationOptions();
        })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
