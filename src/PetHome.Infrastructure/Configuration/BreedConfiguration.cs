using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetEntity;
using PetHome.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Infrastructure.Configuration
{
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
}
