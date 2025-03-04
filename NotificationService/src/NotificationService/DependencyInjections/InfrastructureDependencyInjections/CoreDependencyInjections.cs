using NotificationService.Core.Options;
using NotificationService.Infrastructure.TelegramNotification;
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
        services.Configure<TelegramOption>(configuration.GetSection(TelegramOption.SECTION_NAME));
        return services;
    }

    public static IServiceCollection AddInfrastructureTools(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<TelegramManager>();  

        return services;
    }
}
