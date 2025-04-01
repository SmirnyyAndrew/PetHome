namespace VolunteerRequestService.WEB.DI;

public static class GeneralDependencyInjections
{
    public static IServiceCollection AddDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureDependencyInjection(configuration);
        services.AddApplicationDependencyInjection(configuration);

        return services;
    }
}
