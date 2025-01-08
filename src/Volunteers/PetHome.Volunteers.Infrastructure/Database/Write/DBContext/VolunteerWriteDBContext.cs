using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Species.Infrastructure.Database.Write.DBContext;

namespace PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

public class VolunteerWriteDbContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<_Species> Species => Set<_Species>();

    public VolunteerWriteDbContext(string connectionString)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerWriteDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("write.configuration") ?? false);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesWriteDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("write.configuration") ?? false);
    }
}