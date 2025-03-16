using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.MessageQueues;
using PetHome.Core.Response.Messaging;
using PetHome.Framework.Database;
using PetHome.SharedKernel.Options.Volunteers;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Features.Contracts;
using PetHome.Volunteers.Contracts.Contracts;
using PetHome.Volunteers.Infrastructure.Contracts;
using PetHome.Volunteers.Infrastructure.Database.Read.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write.Repositories;

namespace PetHome.Volunteers.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(
       this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(_ =>
              new VolunteerWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IVolunteerReadDbContext, VolunteerReadDbContext>(_ =>
              new VolunteerReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedPetManagmentEntitiesContract>();
        services.AddScoped<IGetVolunteerInformationContract, GetVolunteerInformationUsingContract>();
        services.AddScoped<IGetPetInformationContract,GetPetInformationUsingContract>();

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.VOLUNTEER_UNIT_OF_WORK_KEY);
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();
        services.Configure<RabbitMqOption>(configuration.GetSection(RabbitMqOption.SECTION_NAME));

        //services.AddHostedService<FilesCleanerHostedService>();
        return services;
    }
}
