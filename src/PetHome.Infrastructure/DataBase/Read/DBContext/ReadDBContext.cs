using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Infrastructure.Shared;

namespace PetHome.Infrastructure.DataBase.Read.DBContext;
 
public class ReadDBContext : DbContext, IReadDBContext
{
    private readonly string _connectionString = Constants.DATABASE;
    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public ReadDBContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    //public ReadDBContext(IConfiguration configuration)
    //{
    //    _connectionString = configuration.GetConnectionString(Constants.DATABASE);
    //}

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDBContext).Assembly,
            type => type.FullName?.Contains("DataBase.Read") ?? false);
    }
}