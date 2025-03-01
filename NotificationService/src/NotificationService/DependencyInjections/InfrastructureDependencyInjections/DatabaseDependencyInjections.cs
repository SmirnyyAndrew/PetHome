using NotificationService.Infrastructure.Database;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class DatabaseDependencyInjections
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<NotificationDbContext>();
        services.AddScoped<NotificationRepository>();
        services.AddScoped<UnitOfWork>();

        return services;
    }
}
