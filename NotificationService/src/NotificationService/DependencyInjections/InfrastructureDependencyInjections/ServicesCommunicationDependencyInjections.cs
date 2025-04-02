using AccountService.Contracts.HttpCommunication;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class ServicesCommunicationDependencyInjections
{
    public static IServiceCollection AddServicesCommunication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountHttpClient();

        return services;
    }
}
