using NotificationService.DependencyInjections.InfrastructureDependencyInjections;

namespace NotificationService.DependencyInjections;

public static class GeneralDependencyInjections
{
    public static IServiceCollection AddDependencyInjections(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }
}
