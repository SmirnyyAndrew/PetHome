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

        services.AddHangFire(configuration);


        services.AddSingleton<IMongoClient>(new MongoClient(
             configuration.GetConnectionString("Mongo")));

        services.AddSingleton<IFilesProvider, MinioProvider>(); 

        services.AddScoped<MongoDbContext>();
        services.AddScoped<MongoDbRepository>();
        services.SetMinioOptions(configuration);


        return services;
    } 
}
