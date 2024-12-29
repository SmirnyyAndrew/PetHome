using Microsoft.EntityFrameworkCore;
using PetHome.API.Inject;
using PetHome.Core.Response.Loggers;
using PetHome.Core.Response.Validation;
using PetHome.SharedKernel.Middlewares;
using PetHome.Volunteers.Infrastructure;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
namespace PetHome.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        //Включить логгер от Serilog
        builder.Services.AddSerilog();
        //Логирование через Seq 
        Log.Logger = SeqLogger.InitDefaultSeqConfiguration();


        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //Auto validation
        builder.Services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });


        //Подключение сервисов
        builder.Services
            .AddInfrastructure(builder.Configuration)
            .AddApplication();


        var app = builder.Build();

        //Middleware для отлова исключений (-стэк трейс)
        app.UseExceptionHandler();

        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //Automigration
            //app.ApplyAutoMigrations();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}

public static class ApplicationExtention
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDBContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static void UseExceptionHandler(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
    }
}