using FilesService.Core.Options;
using Minio;

namespace FilesService.Extentions.BuilderExtentions;

public static class MinioExtentions
{
    public static IServiceCollection SetMinioOptions(
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
