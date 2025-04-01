using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.Infrastructure.Database.Write.DBContext;

public class PetManagementWriteDBContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<Species> Species => Set<Species>();

    public PetManagementWriteDBContext(string connectionString
        = "Host=host.docker.internal;Port=5434;Database=pet_home;Username=postgres;Password=postgres")
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseNpgsql(_connectionString);
        optionBuilder.UseSnakeCaseNamingConvention();
        optionBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionBuilder.EnableSensitiveDataLogging();
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetManagementWriteDBContext).Assembly,
            type => type.FullName?.ToLower().Contains("write.configuration") ?? false); 
    }
}