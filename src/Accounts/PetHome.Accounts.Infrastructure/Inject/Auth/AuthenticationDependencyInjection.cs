using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetHome.Accounts.Application;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Infrastructure.Auth.Jwt;
using PetHome.Accounts.Infrastructure.Database;
using System.Text;

namespace PetHome.Accounts.Infrastructure.Inject.Auth;
public static class AuthenticationDependencyInjection
{
    public static IServiceCollection ApplyAuthenticationAuthorizeConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        JwtOptions _options = configuration.GetSection(JwtOptions.NAME).Get<JwtOptions>()
            ?? throw new ApplicationException("Missing JWT configuration"); ;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
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
                 
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = context =>
                     {
                         Console.WriteLine("Token validated successfully.");
                         return Task.CompletedTask;
                     }
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
