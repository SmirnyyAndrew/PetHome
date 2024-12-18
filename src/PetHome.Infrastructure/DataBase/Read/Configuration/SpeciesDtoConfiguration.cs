using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetHome.Infrastructure.DataBase.Read.Configuration;
public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("specieses");

        //id
        builder.HasKey(x => x.Id);

        //name 
        builder.Property(s => s.Name)
            .IsRequired()
            .HasColumnName("name");

        ////breeds
        //builder.HasMany(s => s.Breeds)
        //    .WithOne()
        //    .HasForeignKey(f => f.SpeciesId);
    }
}
