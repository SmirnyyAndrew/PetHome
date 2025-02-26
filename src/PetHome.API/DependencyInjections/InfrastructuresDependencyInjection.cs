using FilesService.Communication;
using PetHome.Accounts.Infrastructure.Inject;
using PetHome.Discussions.Infrastructure.Inject;
using PetHome.Species.Infrastructure.DI;
using PetHome.VolunteerRequests.Infrastructure.Inject;
using PetHome.Volunteers.Infrastructure.DI;

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

        services.AddAmazonHttpCommunication(configuration);
        services.AddMinioHttpCommunication(configuration);

        return services;
    }
}
