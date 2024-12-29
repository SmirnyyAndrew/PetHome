using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

public class VolunteerWriteDBContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<_Species> Species=> Set<_Species>();

    public VolunteerWriteDBContext(string connectionString)
    {
        _connectionString = connectionString;
    } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    { 
        optionBuilder.UseNpgsql(_connectionString);
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerWriteDBContext).Assembly,
            type => type.FullName?.Contains("DataBase.Write") ?? false);
    }
}