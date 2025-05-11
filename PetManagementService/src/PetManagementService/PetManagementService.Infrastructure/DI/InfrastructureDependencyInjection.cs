using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Infrastructure.MessageBus;
using PetHome.Core.Infrastructure.MessageBus.MessageQueues;
using PetHome.Core.Web.Options.MessageBus;
using PetHome.SharedKernel.Constants;
using PetManagementService.Application.Database;
using PetManagementService.Application.Features.Contracts.PetEntity;
using PetManagementService.Application.Features.Contracts.VolunteerEntity;
using PetManagementService.Contracts.Contracts.Pet;
using PetManagementService.Contracts.Contracts.VolunteerEntity;
using PetManagementService.Infrastructure.Contracts.HardDeleteExpiredSoftDeletedEntities;
using PetManagementService.Infrastructure.Database.Read.DBContext;
using PetManagementService.Infrastructure.Database.Write;
using PetManagementService.Infrastructure.Database.Write.DBContext;
using PetManagementService.Infrastructure.Database.Write.Repositories;

namespace PetManagementService.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddPetManagementInfrastructure(
       this IServiceCollection services, IConfiguration configuration)
    {  
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        services.AddScoped(_ =>
              new PetManagementWriteDBContext(configuration.GetConnectionString(Constants.Database.DATABASE)!));
        services.AddScoped<IPetManagementReadDbContext, PetManagementReadDBContext>(_ =>
              new PetManagementReadDBContext(configuration.GetConnectionString(Constants.Database.DATABASE)!));

        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedSpeciesEntitiesContract>();
        services.AddScoped<IGetVolunteerInformationContract, GetVolunteerInformationUsingContract>();
        services.AddScoped<IGetPetInformationContract, GetPetInformationUsingContract>();

        services.AddScoped<IVolunteerRepository, VolunteerRepository>(); 
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();
        services.Configure<RabbitMqOption>(configuration.GetSection(RabbitMqOption.SECTION_NAME));

        services.Configure<RabbitMqOption>(configuration.GetSection(RabbitMqOption.SECTION_NAME));
        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedSpeciesEntitiesContract>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();

        return services;
    }
}
