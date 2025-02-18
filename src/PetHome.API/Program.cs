using PetHome.Accounts.Infrastructure.Inject.Auth;
using PetHome.API.DependencyInjections;
using PetHome.API.DependencyInjections.AppExtentions;
using PetHome.API.MinimumApi;
using PetHome.Core.Response.Loggers;
using PetHome.Core.Response.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetHome.API;
public partial class Program
{
    public static void Main(string[] args)
    {
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        //Включить логгер от Serilog
        builder.Services.AddSerilog();
        //Логирование через Seq 
        Log.Logger = SeqLogger.InitDefaultSeqConfiguration(builder.Configuration);


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

        //Подключение swagger с возможностью аутентификации
        builder.Services.AddSwaggerGetWithAuthentication();

        //Подключение аутентификации
        builder.Services.ApplyAuthenticationAuthorizeConfiguration(builder.Configuration);

        //Подключение infrastructures
        builder.Services.AddModulesInfrastructures(builder.Configuration);

        //Подключение handlers
        builder.Services.AddModulesServices(builder.Configuration);



        var app = builder.Build();

        //Добавить CORS
        app.AddCORS("http://localhost:5173");

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

        //Логирование запросов
        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        //Добавить минимум-api
        app.AddMinimumApi();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}
public partial class Program;
