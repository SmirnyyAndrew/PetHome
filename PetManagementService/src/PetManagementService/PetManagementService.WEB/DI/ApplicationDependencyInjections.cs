using PetManagementService.Application;
using PetManagementService.WEB.DI.ApplicationDI;

namespace PetManagementService.WEB.DI;

public static class ApplicationDependencyInjections
{
    public static IServiceCollection AddApplicationDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPetManagementServices();
        services.AddRedis(configuration);
        services.AddMassTransitConfig(configuration);
        services.AddOpenTelemetryMetrics();
        services.AddControllers();
        services.AddCors();
        services.ApplyAuthenticationAuthorizeConfiguration(configuration);
        services.AddSwaggerGetWithAuthentication();
        services.AddGraphQLDependencyInjection(); 
        //services.AddFluentValidationAutoValidation(configuration =>
        //{
        //    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        //}); 

        return services;
    }
}
