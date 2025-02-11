using Hangfire;
using Hangfire.PostgreSql;

namespace FilesService.Extentions.BuilderExtentions;

public static class HangFireExtention
{ 
    public static IServiceCollection AddHangFire(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"))));

        //Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }
}
