using Amazon.S3; 

namespace FilesService.Extentions;

public static class AmazonS3Extention
{
    public static IServiceCollection AddAmazonS3(
        this IServiceCollection services, string serviceUrl = "http://localhost:9000/")
    {
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config()
            {
                //Или 9000
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                UseHttp = true,

            };
            return new AmazonS3Client("minioadmin", "minioadmin", config);
        });

        return services;
    }
}
