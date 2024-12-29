using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Species.Application.Database;

namespace PetHome.Species.Infrastructure.Database.Read.DbContext;

public class SpeciesReadDbContext : DbContext, ISpeciesReadDbContext
{
    private readonly string _connectionString = Constants.DATABASE;
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public SpeciesReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseNpgsql(_connectionString);
        optionBuilder.UseSnakeCaseNamingConvention();
        optionBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionBuilder.EnableSensitiveDataLogging();
        optionBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        //Interceptor пока не нужен
        //optionBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesReadDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("database.read") ?? false);
    }
}