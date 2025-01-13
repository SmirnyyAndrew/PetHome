using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Infrastructure.Database.Read.DBContext;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;

namespace PetHome.VolunteerRequests.Infrastructure.Inject;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddVolunteerRequestInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VolunteerRequestDbContext>(_ =>
            new VolunteerRequestDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IVolunteerRequestReadDbContext, VolunteerRequestReadDbContext>(_ =>
            new VolunteerRequestReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IVolunteerRequestRepository, VolunteerRequestRepository>();

        return services;
    }
}
