using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;

namespace PetHome.Species.Infrastructure.Database.Write.Configuration;
public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        //id
        builder.HasKey(x => x.Id);
        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value).Value)
            .IsRequired()
            .HasColumnName("id");

        //name
        builder.Property(i => i.Name)
           .HasConversion(
               name => name.Value,
               value => BreedName.Create(value).Value)
           .IsRequired()
           .HasColumnName("name");

        //softdeletable
        builder.Property(d => d.DeletionDate)
            .HasColumnName("soft_deleted_date");

        builder.Property(d => d.IsDeleted)
            .HasColumnName("is_deleted");
    }
}
