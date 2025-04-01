using PetHome.Core.Redis;

namespace AccountService.WEB.DI.ApplicationDI;

public static class CacheExtentions
{
    public static IServiceCollection AddRedis(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            string connectionString = configuration.GetConnectionString("Redis")
                    ?? throw new ArgumentNullException(nameof(configuration));

            options.Configuration = connectionString;
        });

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
