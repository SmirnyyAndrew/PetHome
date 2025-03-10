using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Application.Features.Contracts;
using PetHome.Discussions.Contracts.Contracts;
using PetHome.Discussions.Infrastructure.Database.Read.DBContext;
using PetHome.Discussions.Infrastructure.Database.Write;
using PetHome.Discussions.Infrastructure.Database.Write.Repositories;
using PetHome.Framework.Database;

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

        services.AddScoped<IGetDiscussionContract, GetDiscussionUsingContract>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.DISCUSSION_UNIT_OF_WORK_KEY);

        return services;
    }
}
