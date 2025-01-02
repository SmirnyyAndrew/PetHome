using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.SharedKernel.Options;
using static CSharpFunctionalExtensions.Result;

namespace PetHome.Accounts.Infrastructure.Inject;
public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(option => configuration.GetSection(JwtOptions.NAME).Get<JwtOptions>());
        services.AddScoped<AuthorizationDbContext>();

        return services;
    }
}
