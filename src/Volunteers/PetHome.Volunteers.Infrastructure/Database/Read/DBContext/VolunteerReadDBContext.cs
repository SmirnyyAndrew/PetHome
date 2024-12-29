using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetHome.Core.Constants;
using PetHome.Volunteers.Application.Database;

namespace PetHome.Volunteers.Infrastructure.Database.Read.DbContext;
 
public class VolunteerReadDbContext : DbContext, IVolunteerReadDbContext
{
    private readonly string _connectionString;
    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public VolunteerReadDbContext(string connectionString = "Host=host.docker.internal;Port=5434;Database=pet_home;Username=postgres;Password=postgres")
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerReadDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("database.rRead") ?? false);
    } 
}