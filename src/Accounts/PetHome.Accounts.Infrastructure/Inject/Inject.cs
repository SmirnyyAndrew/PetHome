using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;

namespace PetHome.Accounts.Infrastructure.Inject;
public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>(_ => new AuthorizationDbContext(configuration.GetConnectionString(Constants.DATABASE)));
         
        return services;
    }
}
