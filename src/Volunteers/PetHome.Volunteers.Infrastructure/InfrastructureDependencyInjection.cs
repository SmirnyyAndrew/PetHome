using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.MessageQueues;
using PetHome.Core.Response.Messaging;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Infrastructure.Background;
using PetHome.Volunteers.Infrastructure.Contracts;
using PetHome.Volunteers.Infrastructure.Database.Read.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write.Repositories;

namespace PetHome.Volunteers.Infrastructure;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(
       this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VolunteerWriteDbContext>(_ =>
              new VolunteerWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IVolunteerReadDbContext, VolunteerReadDbContext>(_ =>
              new VolunteerReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedPetManagmentEntitiesContract>();
         
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();  
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.VOLUNTEER_UNIT_OF_WORK_KEY);
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();
        
        //services.AddHostedService<FilesCleanerHostedService>();
        return services;
    } 
}
