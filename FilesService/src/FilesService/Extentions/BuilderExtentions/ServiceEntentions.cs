using FilesService.Infrastructure.MongoDB;
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

        return services;
    }
}
