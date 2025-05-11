using PetManagementService.Application.Features.GraphQL.PetAggregate;
using PetManagementService.Application.Features.GraphQL.Query;
using PetManagementService.Application.Features.GraphQL.VolunteerAggregate;
using PetManagementService.Infrastructure.Database.Read.DBContext;

namespace PetManagementService.WEB.DI.ApplicationDI;

public static class GraphQLExtention
{
    public static IServiceCollection AddGraphQLDependencyInjection(
        this IServiceCollection services)
    {
        services.AddGraphQLServer();

        services.AddScoped<PetGraphQLService>();
        services.AddScoped<VolunteerGraphQLService>();

        services.AddGraphQLServer()
            .AddType<PetGraphQLType>()
            .AddType<VolunteerGraphQLType>()
            .AddQueryType<PetManagementGraphQLQuery>()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        return services;
    }
}
