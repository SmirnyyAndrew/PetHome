using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Domain.PetEntity;
using PetHome.Domain.VolunteerEntity;
using PetHome.Infrastructure.Configuration;

namespace PetHome.Infrastructure
{
    public class ApplicationDBContext(IConfiguration configuration) : DbContext
    {
        private const string DATABASE = "PetHomeConnectionString";

        public DbSet<Volunteer> Volunteers { get; set; } 
        public DbSet<Pet> Pets{ get; set; } 
        //public DbSet<PetShelter> PetShelters { get; set; } 
        //public DbSet<Breed> Breeds { get; set; } 
        //public DbSet<Species> Species{ get; set; } 


        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //optionBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE)); 
            optionBuilder.UseNpgsql("Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=postgres");
            optionBuilder.UseSnakeCaseNamingConvention();
            optionBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfiguration(new VolunteerConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

    }
}
