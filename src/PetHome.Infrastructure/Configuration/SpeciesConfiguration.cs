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

            builder.HasKey(x => x.Id);

            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value).Value)
                .IsRequired()
                .HasColumnName("Id");

            builder.ComplexProperty(s => s.Name, tb =>
            {
                tb.Property(v=>v.Value)
                .IsRequired()
                .HasColumnName("name");
            }); 
        }
    }
}
