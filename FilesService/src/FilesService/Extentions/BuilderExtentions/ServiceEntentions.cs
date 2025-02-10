using FilesService.Application.Interfaces;
using FilesService.Core.Options;
using FilesService.Infrastructure.Minio;
using FilesService.Infrastructure.MongoDB;
using Minio;
using MongoDB.Driver;

namespace FilesService.Extentions.BuilderExtentions;

public static class ServiceExtentions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints();

        services.AddCors();

        services.AddAmazonS3(configuration);

        services.AddSingleton<IMongoClient>(new MongoClient(
             configuration.GetConnectionString("Mongo")));

        services.AddScoped<MongoDbContext>();
        services.AddScoped<MongoDbRepository>();
        services.SetMinioOptions(configuration);
        services.AddSingleton<IFilesProvider, MinioProvider>(); 

        return services;
    }

    private static IServiceCollection SetMinioOptions(
        this IServiceCollection services, IConfiguration configuration)
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
