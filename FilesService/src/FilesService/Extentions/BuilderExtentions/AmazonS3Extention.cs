using Amazon.S3;
using FilesService.Core.Options;

namespace FilesService.Extentions.BuilderExtentions;

public static class AmazonS3Extention
{
    public static IServiceCollection AddAmazonS3(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var minioOptions = configuration.GetSection(MinioOptions.MINIO_NAME).Get<MinioOptions>()
            ?? throw new Exception("Ошибка со строкой подключения minio. Проверьте конфигурацию.");

        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config()
            { 
                ServiceURL = minioOptions.Endpoint,
                ForcePathStyle = true,
                UseHttp = true,
            };
            return new AmazonS3Client(minioOptions.Accesskey, minioOptions.Secretkey, config);
        });

        return services;
    }
}
