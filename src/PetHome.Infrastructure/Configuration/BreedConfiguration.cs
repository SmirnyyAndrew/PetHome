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

            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .HasColumnName("id");


            builder.Property(i => i.Name)
                .HasConversion(
                    id => id.Value,
                    value => BreedName.Create(value).Value)
                .HasColumnName("name");
        }
    }
}
