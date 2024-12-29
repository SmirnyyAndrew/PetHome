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
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure;
using PetHome.Species.Infrastructure.Database.Read.DBContext;
using PetHome.Species.Infrastructure.Database.Write;
using PetHome.Species.Infrastructure.Database.Write.DbContext;
using PetHome.Species.Infrastructure.Database.Write.Repositories;

namespace PetHome.Species.Infrastructure;
public static class Inject
{
    public static IServiceCollection AddSpeciesInfrastructure(
       this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<SpeciesWriteDBContext>(_ =>
              new SpeciesWriteDBContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
              new SpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        services.AddMinio(configuration);
        services.AddSingleton<IFilesProvider, MinioProvider>();  
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.SPECIES_UNIT_OF_WORK_KEY);
        services.AddSingleton<IMessageQueue, FilesCleanerMessageQueue>();
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
