using FluentValidation.AspNetCore;
using AccountService.WEB.DI.ApplicationDI;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using AccountService.Application;

namespace AccountService.WEB.DI;

public static class ApplicationDependencyInjections
{
    public static IServiceCollection AddApplicationDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountsServices();
        services.AddRedis(configuration);
        services.AddMassTransitConfig(configuration);
        services.AddOpenTelemetryMetrics();
        services.AddControllers();
        services.AddCors(); 
        services.AddSwaggerGetWithAuthentication();
        //services.AddFluentValidationAutoValidation(configuration =>
        //{
        //    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        //});


        return services;
    }
}
