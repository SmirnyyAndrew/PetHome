using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Infrastructure.Database.Read.DBContext;
using PetHome.Discussions.Infrastructure.Database.Write;
using PetHome.Discussions.Infrastructure.Database.Write.Repositories;

namespace PetHome.Discussions.Infrastructure.Inject;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddDiscussionInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DiscussionDbContext>(_ =>
            new DiscussionDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IDiscussionReadDbContext, DiscussionReadDbContext>(_ =>
            new DiscussionReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IDiscussionRepository, DiscussionRepository>();

        return services;
    }
}
