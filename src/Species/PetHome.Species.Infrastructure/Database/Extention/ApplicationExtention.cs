using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHome.SharedKernel.Middlewares;
using PetHome.Species.Infrastructure.Database.Write.DBContext;

namespace PetHome.Species.Infrastructure.Database.Extention;

public static class ApplicationExtention
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static void UseExceptionHandler(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
    }
}
