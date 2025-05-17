using PetManagementService.WEB.DI.AppDI;

namespace PetManagementService.WEB.DI;

public static class AppDependencyInjections
{
    public static WebApplication AddAppDependencyInjection(
        this WebApplication app)
    {
        app.AddCORS("http://localhost:5173", "http://localhost:3000", "http://localhost:8888");
        app.UseExceptionHandler();
        app.AddGRPCServices(); 
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.MapGraphQL();
        return app;
    }
}
