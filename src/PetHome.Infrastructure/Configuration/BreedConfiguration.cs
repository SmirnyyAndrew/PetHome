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

            builder.HasKey(x => x.Id);

            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .IsRequired()
                .HasColumnName("id");

            builder.ComplexProperty(i => i.Name, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("name");
            });

            builder.ComplexProperty(s => s.SpeciesId, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("species_id");
            }); 
        }
    }
}
