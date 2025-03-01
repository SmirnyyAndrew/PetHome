using NotificationService.Infrastructure.Database.Write;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class DatabaseDependencyInjections
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<NotificationWriteDbContext>();
        services.AddScoped<NotificationRepository>();

        return services;
    }
}
