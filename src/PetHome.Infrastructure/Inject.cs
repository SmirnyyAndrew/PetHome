using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetHome.Application.Features.Volunteers;
using PetHome.Infrastructure.DataBase;
using PetHome.Infrastructure.DataBase.Repositories;
using PetHome.Infrastructure.Options;
using MinioOptions = PetHome.Infrastructure.Options.MinioOptions;

namespace PetHome.Infrastructure;
public static class Inject
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<ApplicationDBContext>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddMinio(configuration);
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
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.IsWithSSL);
        });
        return services;
    }

}
