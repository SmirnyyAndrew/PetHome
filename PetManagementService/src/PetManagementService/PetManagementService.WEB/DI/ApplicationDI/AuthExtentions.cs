using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;

namespace PetManagementService.WEB.DI.ApplicationDI;

public static class AuthExtentions
{
    public static IServiceCollection ApplyAuthenticationAuthorizeConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        //Аутентификация
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
                 //options.TokenValidationParameters = TokenValidationManager.GetTokenValidationParameters(configuration);

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
        //Авторизация
        services.AddAuthorization();

        ////Jwt token
        //services.AddTransient<ITokenProvider, TokenProvider>();

        ////Identity
        //services.AddIdentity<User, Role>(options =>
        //{
        //    options.GetAuthenticationOptions();
        //})
        //    .AddEntityFrameworkStores<AuthorizationDbContext>()
        //    .AddDefaultTokenProviders();

        ////Сидирование
        //services.AddSeedings(configuration);

        return services;
    }
}
