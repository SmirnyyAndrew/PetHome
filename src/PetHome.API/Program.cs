using PetHome.API.Extentions;
using PetHome.API.Loggers;
using PetHome.API.Validation;
using PetHome.Application;
using PetHome.Infrastructure;
using Serilog;
using Serilog.Events;
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
            .AddInfrastructure()
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
            app.ApplyAutoMigrations();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
