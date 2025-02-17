using FilesService.Communication.HttpClients;
using FilesService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FilesService.Communication;
public static class FilesServiceExtentions
{
    public static IServiceCollection AddAmazonHttpCommunication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FilesServiceOptions>(configuration.GetSection(FilesServiceOptions.SECTION_NAME));

        services.AddHttpClient<IAmazonFilesHttpClient, AmazonFilesHttpClient>((sp, config) =>
        {
            var filesServiceOptions = sp.GetRequiredService<IOptions<FilesServiceOptions>>().Value;
            config.BaseAddress = new Uri(filesServiceOptions.Url);
        });

        services.AddSingleton<IAmazonFilesHttpClient, AmazonFilesHttpClient>();

        return services;
    }

    public static IServiceCollection AddMinioHttpCommunication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FilesServiceOptions>(configuration.GetSection(FilesServiceOptions.SECTION_NAME));

        services.AddHttpClient<IMinioFilesHttpClient, MinioFilesHttpClient>((sp, config) =>
        {
            var filesServiceOptions = sp.GetRequiredService<IOptions<FilesServiceOptions>>().Value;
            config.BaseAddress = new Uri(filesServiceOptions.Url);
        });

        services.AddSingleton<IMinioFilesHttpClient, MinioFilesHttpClient>();

        return services;
    }
}
