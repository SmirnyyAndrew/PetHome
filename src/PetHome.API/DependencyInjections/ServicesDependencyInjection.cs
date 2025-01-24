using PetHome.Accounts.Application;
using PetHome.Discussions.Application.Inject;
using PetHome.Species.Application;
using PetHome.Volunteers.Application;

namespace PetHome.API.DependencyInjections;

public static class ServicesDependencyInjection
{
    public static IServiceCollection AddModulesServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSpeciesServices()
            .AddVolunteerServices()
            .AddAccountsServices()
            .AddDiscussionServices();

        return services;
    }
}
