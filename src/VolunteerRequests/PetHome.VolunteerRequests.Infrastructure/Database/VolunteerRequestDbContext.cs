using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Infrastructure.Database;
public class VolunteerRequestDbContext : DbContext
{
    DbSet<VolunteerRequest> VolunteerRequests => Set<VolunteerRequest>();

    private readonly string _conntecitonString;

    public VolunteerRequestDbContext(string conntecitonString
        = "Host=host.docker.internal;Port=5434;Database=pet_home;Username=postgres;Password=postgres")
    {
        _conntecitonString = conntecitonString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_conntecitonString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }


    private ILoggerFactory CreateLoggerFactory() =>
       LoggerFactory.Create(builder => { builder.AddConsole(); });


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("VolunteerRequests");

        builder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("database.configuration") ?? false);  
    }
}
