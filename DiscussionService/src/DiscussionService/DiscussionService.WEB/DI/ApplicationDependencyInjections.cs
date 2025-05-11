using DiscussionService.Application.Inject;
using DiscussionService.WEB.DI.ApplicationDI;

namespace DiscussionService.WEB.DI;

public static class ApplicationDependencyInjections
{
    public static IServiceCollection AddApplicationDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDiscussionServices();
        services.AddRedis(configuration);
        services.AddMassTransitConfig(configuration);
        services.AddOpenTelemetryMetrics();
        services.AddControllers();
        services.AddCors();
        services.ApplyAuthenticationAuthorizeConfiguration(configuration);
        services.AddSwaggerGetWithAuthentication();
        //services.AddFluentValidationAutoValidation(configuration =>
        //{
        //    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        //});


        return services;
    }
}
