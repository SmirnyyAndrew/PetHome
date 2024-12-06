using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Volunteers;
using PetHome.Infrastructure.DataBase;
using PetHome.Infrastructure.DataBase.Repositories;

namespace PetHome.Infrastructure;
public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDBContext>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        return services;
    }
}
