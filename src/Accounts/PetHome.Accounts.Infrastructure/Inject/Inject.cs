using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Accounts.Infrastructure.Permission;
using PetHome.Core.Constants;

namespace PetHome.Accounts.Infrastructure.Inject;
public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>(_ =>
            new AuthorizationDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationRequirement, PermissionRequirement>();
        services.AddScoped<AuthorizeAttribute, PermissionAttribute>();
        services.AddScoped<AuthorizationHandler<PermissionRequirement>, PermissionRequirementHandler>();

        return services;
    }
}
