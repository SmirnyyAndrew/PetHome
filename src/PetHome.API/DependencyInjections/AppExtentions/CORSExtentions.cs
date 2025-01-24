namespace PetHome.API.DependencyInjections.AppExtentions;

public static class CORSExtentions
{
    public static WebApplication AddCORS(
        this WebApplication app, params string[] urls)
    {
        app.UseCors(config =>
        {
            config.WithOrigins(urls)
            .AllowAnyHeader()
            .AllowAnyMethod();
        });

        return app;
    }
}
