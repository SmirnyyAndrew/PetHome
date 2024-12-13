using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Infrastructure.Interceptors;

namespace PetHome.Infrastructure.DataBase;

public class ApplicationDBContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "PetHomeConnectionString";

    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<Species> Specieses => Set<Species>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionBuilder.UseSnakeCaseNamingConvention();
        optionBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionBuilder.EnableSensitiveDataLogging();
        //Interceptor пока не нужен
        //optionBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
    }
}