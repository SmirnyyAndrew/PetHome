using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Infrastructure.Database.Read.Configuration;
public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("specieses");
        builder.Property(s => s.Id) 
            .IsRequired()
            .HasColumnName("Id");

        //id
        builder.HasKey(x => x.Id);

        //name 
        builder.Property(s => s.Name)
            .IsRequired()
            .HasColumnName("name");

        //breeds
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(f => f.SpeciesId);
    }
}
