using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Infrastructure.Database.Write.Configuration;
public class SpeciesConfiguration : IEntityTypeConfiguration<_Species>
{
    public void Configure(EntityTypeBuilder<_Species> builder)
    {
        builder.ToTable("specieses");

        //id
        builder.HasKey(x => x.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value).Value)
            .IsRequired()
            .HasColumnName("Id");

        //name 
        builder.Property(s => s.Name)
            .HasConversion(
                n => n.Value,
                value => SpeciesName.Create(value).Value)
            .IsRequired()
            .HasColumnName("name");

        //breeds
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(x => x.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);

        //soft delete 
        builder.Property(d => d.IsDeleted)
            .HasColumnName("is_deleted");

        builder.Property(d => d.DeletionDate)
            .HasColumnName("soft_deleted_date");
    }
}
