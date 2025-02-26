using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.MessageQueues;
using PetHome.Core.Response.Messaging;
using PetHome.Framework.Database;
using PetHome.SharedKernel.Options.Volunteers;
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure.Contracts.HardDeleteExpiredSoftDeletedEntities;
using PetHome.Species.Infrastructure.Database.Read.DBContext;
using PetHome.Species.Infrastructure.Database.Write;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using PetHome.Species.Infrastructure.Database.Write.Repositories;

namespace PetHome.Species.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(
       this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(_ =>
              new SpeciesWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
              new SpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        services.Configure<RabbitMqOption>(configuration.GetSection(RabbitMqOption.SECTION_NAME));
        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedSpeciesEntitiesContract>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.SPECIES_UNIT_OF_WORK_KEY);
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();

        return services;
    }
}
