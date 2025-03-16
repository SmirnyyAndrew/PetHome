using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Application.Features.Contracts;
using PetHome.VolunteerRequests.Contracts.Contracts;
using PetHome.VolunteerRequests.Infrastructure.Database.Read.DBContext;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;
using PetHome.VolunteerRequests.Infrastructure.Database.Write.Repositories;

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
       
        services.AddScoped<IGetVolunteerRequestContract, GetVolunteerRequestUsingContract>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY);

        return services;
    }
}
