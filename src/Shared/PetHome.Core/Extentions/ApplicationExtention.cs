using Microsoft.EntityFrameworkCore;
using PetHome.API.Middlewares;
using PetHome.Infrastructure.DataBase.Write.DBContext;

namespace PetHome.API.Extentions;

public static class ApplicationExtention
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDBContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static void UseExceptionHandler(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
    }
}
