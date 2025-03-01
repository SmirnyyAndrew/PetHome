using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;

namespace NotificationService.Database.Write;

public class NotificationWriteDbContext : DbContext
{
    public DbSet<UserNotificationSettings> Notifications => Set<UserNotificationSettings>();
    public readonly string _connectionString;

    public NotificationWriteDbContext(string connectionString
        = "Host=host.docker.internal;Port=5434;Database=pet_home;Username=postgres;Password=postgres")
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    private ILoggerFactory? CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Notifications");

        builder.ApplyConfigurationsFromAssembly(typeof(NotificationWriteDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("write.configuration") ?? false);
    }
}
