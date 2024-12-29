using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetHome.Core.Interfaces;
using PetHome.Volunteers.Application.Database.RepositoryInterfaces;
using PetHome.Volunteers.Infrastructure.Database.Write;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write.Repositories;
using MinioOptions = PetHome.Infrastructure.Options.MinioOptions;
using PetHome.Volunteers.Infrastructure;
using PetHome.Volunteers.Infrastructure.Background;
using PetHome.Volunteers.Infrastructure.Database.Read.DBContext;

namespace PetHome.Volunteers.Infrastructure;
public static class Inject
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<WriteDBContext>(_ =>
              new WriteDBContext(configuration.GetConnectionString(Constants.DATABASE)!));
        services.AddScoped<IReadDBContext, ReadDBContext>(_ =>
              new ReadDBContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        services.AddMinio(configuration);
        services.AddSingleton<IFilesProvider, MinioProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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
