using NotificationService.DependencyInjections.ApplicationDependencyInjections;
using NotificationService.DependencyInjections.InfrastructureDependencyInjections;

namespace NotificationService.DependencyInjections;

public static class GeneralDependencyInjections
{
    public static IServiceCollection AddDependencyInjections(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddServices();
        services.AddOptions(configuration);
        services.AddInfrastructureTools(configuration);
        services.AddMassTransitConfig(configuration); 
        services.AddGRPCClients();

        return services;
    }
}
