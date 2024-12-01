using PetHome.Application;
using PetHome.Application.Volunteers;
using PetHome.Application.Volunteers.CreateVolunteer;
using PetHome.Infrastructure;
using PetHome.Infrastructure.Repositories;

namespace PetHome.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //Подключение сервисов
        builder.Services
            .AddInfrastructure()
            .AddApplication();


        var app = builder.Build();
         
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run(); 
    }
}
