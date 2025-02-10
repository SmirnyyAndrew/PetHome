using PetHome.SharedKernel.Middlewares;

namespace FilesService.Extentions;

public static class ExceptionMiddlewareExtention
{
    public static WebApplication UseExceptionHandler(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
