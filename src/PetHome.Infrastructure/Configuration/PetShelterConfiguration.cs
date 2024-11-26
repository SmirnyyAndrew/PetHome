using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Infrastructure.Configuration
{
    public class PetShelterConfiguration : IEntityTypeConfiguration<PetShelter>
    {
        public void Configure(EntityTypeBuilder<PetShelter> builder)
        {
            builder.ToTable("pet_shelters");

            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => PetShelterId.Create(value))
                .IsRequired();

            builder.HasMany(i => i.PetList)
                .WithOne()
                .HasForeignKey("pet_shelter_id")
                .IsRequired();
        }
    }
}
