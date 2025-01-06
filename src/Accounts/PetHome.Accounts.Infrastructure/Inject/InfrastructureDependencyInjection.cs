using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Infrastructure.Auth.Permissions;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Accounts.Infrastructure.Database.Seed;
using PetHome.Core.Auth;
using PetHome.Core.Constants;

namespace PetHome.Accounts.Infrastructure.Inject;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>(_ =>
            new AuthorizationDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();  

        return services;
    }
}
