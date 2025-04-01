using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Infrastructure.Database.Read.DBContext;

public class PetManagementReadDBContext : DbContext, IPetManagementReadDbContext
{
    private readonly string _connectionString;
    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public PetManagementReadDBContext(string connectionString
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
            optionBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetManagementReadDBContext).Assembly,
            type => type.FullName?.ToLower().Contains("read.configuration") ?? false); 
    } 
}