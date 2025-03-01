using NotificationService.Core.Options;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class CoreDependencyInjections
{
    public static IServiceCollection AddOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOption>(configuration.GetSection(EmailOption.GOOGLE));
        services.Configure<EmailOption>(configuration.GetSection(EmailOption.YANDEX)); 
        return services;
    }
}
