using PetHome.Core.Web.Middlewares;

namespace AccountService.WEB.DI.AppDI;

public static class ExceptionMiddlewareExtention
{
    public static WebApplication UseExceptionHandler(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
