using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.VolunteerRequests.Application.Database.Dto;
using PetHome.VolunteerRequests.Application.Database.Interfaces;

namespace PetHome.VolunteerRequests.Infrastructure.Database.Read.DBContext;
public class VolunteerRequestReadDbContext : DbContext, IVolunteerRequestReadDbContext
{
    public IQueryable<VolunteerRequestDto> VolunteerRequests => Set<VolunteerRequestDto>();

    private readonly string _conntecitonString;

    public VolunteerRequestReadDbContext(string conntecitonString
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

        builder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestReadDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("read.configuration") ?? false);
    }
}
