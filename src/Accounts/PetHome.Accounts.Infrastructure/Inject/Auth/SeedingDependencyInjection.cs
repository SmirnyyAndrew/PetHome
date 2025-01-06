using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Infrastructure.Database.Seed;
using PetHome.Accounts.Infrastructure.Database.Seedings;

namespace PetHome.Accounts.Infrastructure.Inject.Auth;
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
