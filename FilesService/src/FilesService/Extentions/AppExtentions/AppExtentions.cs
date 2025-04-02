using FilesService.Extentions.BuilderExtentions;
using Hangfire;

namespace FilesService.Extentions.AppExtentions;

public static class AppExtentions
{
    public static WebApplication AddAppAttributes(this WebApplication app)
    { 
        //Добавить CORS
        app.AddCORS("http://localhost:5173");
         
        //Middleware для отлова исключений (-стэк трейс)
        app.UseExceptionHandler();
         
        app.MapEndpoints();
        
        app.UseHangfireServer();
        app.UseHangfireDashboard();

        return app;
    }
}
