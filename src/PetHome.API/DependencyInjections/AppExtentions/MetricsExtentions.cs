using MassTransit.Logging;
using MassTransit.Monitoring;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PetHome.API.DependencyInjections.AppExtentions;

public static class MetricsExtentions
{
    public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services)
    {
        string meterName = "smirnyy";

        services.AddOpenTelemetry()
            .ConfigureResource(resourse => resourse.AddService(nameof(PetHome.API)))
        //prometheus
            .WithMetrics(metric => metric
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(PetHome.API)))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter()
                .AddMeter(InstrumentationOptions.MeterName))
        //jaeger 
            .WithTracing(tracingBuilder =>
            {
                tracingBuilder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(PetHome.API)))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddSource(DiagnosticHeaders.DefaultListenerName)
                    .AddOtlpExporter(c => c.Endpoint = new Uri("http://localhost:4317"));
            });

        return services;
    }
}
