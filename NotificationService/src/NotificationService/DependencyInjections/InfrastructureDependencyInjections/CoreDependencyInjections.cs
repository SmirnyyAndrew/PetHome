using NotificationService.Core.Options;
using PetHome.SharedKernel.Options.Volunteers;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class CoreDependencyInjections
{
    public static IServiceCollection AddOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOption>(configuration.GetSection(EmailOption.GOOGLE));
        services.Configure<EmailOption>(configuration.GetSection(EmailOption.YANDEX));
        services.Configure<RabbitMqOption>(configuration.GetSection(RabbitMqOption.SECTION_NAME));
        return services;
    }
}
