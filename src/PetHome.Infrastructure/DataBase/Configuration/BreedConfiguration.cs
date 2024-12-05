using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Infrastructure.DataBase.Configuration;
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
                value => BreedId.Create(value))
            .IsRequired()
            .HasColumnName("id");

        //name
        builder.Property(i => i.Name)
           .HasConversion(
               name => name.Value,
               value => BreedName.Create(value).Value)
           .IsRequired()
           .HasColumnName("name");

        //species id
        builder.Property(i => i.SpeciesId)
           .HasConversion(
               id => id.Value,
               value => SpeciesId.Create(value).Value)
           .IsRequired()
           .HasColumnName("species_id");
    }
}
