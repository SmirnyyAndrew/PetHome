using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Discussions.Application.Database.Dto;
using PetHome.Discussions.Application.Database.Interfaces;

namespace PetHome.Discussions.Infrastructure.Database.Read.DBContext;
public class DiscussionReadDbContext : DbContext, IDiscussionReadDbContext
{
    public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();
    public IQueryable<RelationDto> Relations => Set<RelationDto>();

    private readonly string _conntecitonString;

    public DiscussionReadDbContext(string conntecitonString
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
        builder.HasDefaultSchema("Discussions");

        builder.ApplyConfigurationsFromAssembly(typeof(DiscussionReadDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("read.configuration") ?? false);
    }
}
