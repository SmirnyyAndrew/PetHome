using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace PetHome.API.DependencyInjections.AppExtentions;

public static class MetricsExtentions
{
    public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services)
    {
        string meterName = "smirnyy";

        services.AddOpenTelemetry()
            .WithMetrics(b => b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(PetHome.API)))
            .AddMeter(meterName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter());
             
        return services;
    }
}
