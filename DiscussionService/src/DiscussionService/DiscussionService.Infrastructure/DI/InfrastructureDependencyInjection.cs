using DiscussionService.Application.Database.Interfaces;
using DiscussionService.Application.Features.Contracts;
using DiscussionService.Contracts.Contracts;
using DiscussionService.Infrastructure.Database.Read.DBContext;
using DiscussionService.Infrastructure.Database.Write;
using DiscussionService.Infrastructure.Database.Write.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Framework.Database;

namespace DiscussionService.Infrastructure.DI;
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
