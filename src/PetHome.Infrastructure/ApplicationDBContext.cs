using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Infrastructure
{
    public class ApplicationDBContext(IConfiguration configuration) : DbContext
    {
        private const string DATABASE = "PetHomeConnectionString";
        
        public DbSet<Volunteer> Volunteers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE)); 
            optionBuilder.UseSnakeCaseNamingConvention();
            optionBuilder.UseLoggerFactory(CreateLoggerFactory());
        }
        
        private ILoggerFactory CreateLoggerFactory ()=>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

    }
}
