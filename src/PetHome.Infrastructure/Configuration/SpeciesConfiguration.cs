using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetEntity;

namespace PetHome.Infrastructure.Configuration
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value).Value)
                .IsRequired();

            builder.Property(s => s.Name)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesName.Create(value).Value)
                .IsRequired();

            builder.HasMany(b => b.BreedList)
                .WithOne()
                .HasForeignKey("species_id")
                .IsRequired();

            builder.HasMany(b => b.PetList)
                .WithOne()
                .HasForeignKey("species_id")
                .IsRequired();

        }
    }
}
