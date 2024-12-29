using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Infrastructure.Database.Write.DbContext;

public class SpeciesWriteDBContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<_Species> Species => Set<_Species>();

    public SpeciesWriteDBContext(string connectionString)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesWriteDBContext).Assembly,
            type => type.FullName?.ToLower().Contains("database.write") ?? false);
    }
}