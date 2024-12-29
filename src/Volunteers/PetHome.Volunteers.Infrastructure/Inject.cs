using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces;
using PetHome.Core.Response.MessageQueues;
using PetHome.Core.Response.Messaging;
using PetHome.Framework.Database;
using PetHome.SharedKernel.Options;
using PetHome.SharedKernel.Providers.Minio;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Infrastructure;
using PetHome.Volunteers.Infrastructure.Background;
using PetHome.Volunteers.Infrastructure.Database.Read.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write.Repositories; 

namespace PetHome.Volunteers.Infrastructure;
public static class Inject
{
    public static IServiceCollection AddVolunteerInfrastructure(
       this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<VolunteerWriteDbContext>(_ =>
              new VolunteerWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IVolunteerReadDbContext, VolunteerReadDbContext>(_ =>
              new VolunteerReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IVolunteerRepository, VolunteerRepository>(); 
        services.AddMinio(configuration);
        services.AddSingleton<IFilesProvider, MinioProvider>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.VOLUNTEER_UNIT_OF_WORK_KEY);
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();
        services.AddHostedService<FilesCleanerHostedService>();
        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        var minioOptions = configuration.GetSection(MinioOptions.MINIO_NAME).Get<MinioOptions>()
            ?? throw new Exception("Ошибка со строкой подключения minio. Проверьте конфигурацию.");

        services.AddMinio(options =>
        {
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.Accesskey, minioOptions.Secretkey);
            options.WithSSL(minioOptions.IsWithSSL);
        });
        return services;
    }

}
