using AccountService.Infrastructure.Database.Seedings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Infrastructure.DI.Auth;
public static class SeedingDependencyInjection
{
    public static IServiceCollection AddSeedings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.SeedRolesWithPermissions(configuration);
        services.SeedAdmins(configuration);

        return services;
    }
}
