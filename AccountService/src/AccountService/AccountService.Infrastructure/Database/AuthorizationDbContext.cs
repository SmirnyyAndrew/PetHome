using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using AccountService.Domain.Others;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.SharedKernel.Responses.RefreshToken;

namespace AccountService.Infrastructure.Database;
public class AuthorizationDbContext : IdentityDbContext<User, Role, Guid>
{
    public override DbSet<User> Users => Set<User>();
    public DbSet<AdminAccount> Admins => Set<AdminAccount>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();
    public override DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolesPermissions => Set<RolePermission>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


    private readonly string _conntecitonString;

    public AuthorizationDbContext(string conntecitonString
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
        builder.HasDefaultSchema("Account");

        builder.ApplyConfigurationsFromAssembly(typeof(AuthorizationDbContext).Assembly,
            type => type.FullName?.ToLower().Contains("database.configuration") ?? false);

        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claim");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_token");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_login");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_role");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claim");

    }
}
