using DiscussionService.Application.Database.Interfaces;
using DiscussionService.Application.Features.Contracts;
using DiscussionService.Contracts.Contracts;
using DiscussionService.Infrastructure.Database.Read.DBContext;
using DiscussionService.Infrastructure.Database.Write;
using DiscussionService.Infrastructure.Database.Write.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.SharedKernel.Constants;
using PetHome.Core.Infrastructure.Database;

namespace DiscussionService.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddDiscussionInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DiscussionDbContext>(_ =>
            new DiscussionDbContext(configuration.GetConnectionString(Constants.Database.DATABASE)!));
        services.AddScoped<IDiscussionReadDbContext, DiscussionReadDbContext>(_ =>
            new DiscussionReadDbContext(configuration.GetConnectionString(Constants.Database.DATABASE)!));

        services.AddScoped<IDiscussionRepository, DiscussionRepository>();

        services.AddScoped<IGetDiscussionContract, GetDiscussionUsingContract>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Database.DISCUSSION_UNIT_OF_WORK_KEY);

        return services;
    }
}
