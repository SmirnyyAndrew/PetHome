using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.VolunteerRequests.Application.Database;
using PetHome.VolunteerRequests.Infrastructure.Database;
using PetHome.VolunteerRequests.Infrastructure.Database.Read.DBContext;

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

        return services;
    }
}
