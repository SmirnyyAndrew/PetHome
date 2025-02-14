using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FilesService.Communication;
public static class FilesServiceExtentions
{
    public static IServiceCollection AddFileHttpCommunication(
        this IServiceCollection services, string sectionName, IConfiguration configuration)
    {
        services.Configure<FilesServiceOptions>(configuration.GetSection(FilesServiceOptions.SECTION_NAME));

        services.AddHttpClient<FilesHttpClient>((sp, config) =>
        {
            var filesServiceOptions = sp.GetRequiredService<IOptions<FilesServiceOptions>>().Value;
            config.BaseAddress = new Uri(filesServiceOptions.Url);
        });

        return services;
    }
}
