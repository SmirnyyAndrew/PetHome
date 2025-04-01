using PetHome.VolunteerRequests.Infrastructure.Inject;

namespace VolunteerRequestService.WEB.DI.InfrastructureDI;

public static class DatabaseDI
{
    public static IServiceCollection AddDatabases(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddVolunteerRequestInfrastructure(configuration);

        return services;
    }
}
