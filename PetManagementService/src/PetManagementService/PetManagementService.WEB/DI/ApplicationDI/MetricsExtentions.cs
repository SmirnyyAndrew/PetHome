using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using MassTransit.Monitoring;
using OpenTelemetry.Trace;
using MassTransit.Logging;

namespace PetManagementService.WEB.DI.ApplicationDI;

public static class MetricsExtentions
{
    public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services)
    {
        string meterName = "smirnyy";

        services.AddOpenTelemetry()
            .ConfigureResource(resourse => resourse.AddService(nameof(WEB)))
        //prometheus
            .WithMetrics(metric => metric
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(WEB)))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter()
                .AddMeter(InstrumentationOptions.MeterName))
        //jaeger 
            .WithTracing(tracingBuilder =>
            {
                tracingBuilder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(WEB)))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    //.AddNpgsql()
                    .AddSource(DiagnosticHeaders.DefaultListenerName)
                    .AddOtlpExporter(c => c.Endpoint = new Uri("http://localhost:4317"));
            });

        return services;
    }
}
