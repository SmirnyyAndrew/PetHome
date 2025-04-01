using DiscussionService.WEB.DI.InfrastructureDI;
using DiscussionService.Infrastructure.DI;

namespace DiscussionService.WEB.DI;

public static class InfrastructureDependencyInjections
{
    public static IServiceCollection AddInfrastructureDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabases(configuration);
        services.AddMessageBus(configuration);
        services.AddDiscussionInfrastructure(configuration);

        return services;
    }
}
