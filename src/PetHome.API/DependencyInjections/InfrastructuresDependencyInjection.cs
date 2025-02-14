using FilesService.Communication;
using PetHome.Accounts.Infrastructure.Inject;
using PetHome.Discussions.Infrastructure.Inject;
using PetHome.Species.Infrastructure;
using PetHome.VolunteerRequests.Infrastructure.Inject;
using PetHome.Volunteers.Infrastructure;

namespace PetHome.API.DependencyInjections;

public static class InfrastructuresDependencyInjection
{
    public static IServiceCollection AddModulesInfrastructures(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAccountsInfrastructure(configuration)
            .AddSpeciesInfrastructure(configuration)
            .AddVolunteerInfrastructure(configuration)
            .AddVolunteerRequestInfrastructure(configuration)
            .AddDiscussionInfrastructure(configuration);

        services.AddFileHttpCommunication(configuration);

        return services;
    }
}
