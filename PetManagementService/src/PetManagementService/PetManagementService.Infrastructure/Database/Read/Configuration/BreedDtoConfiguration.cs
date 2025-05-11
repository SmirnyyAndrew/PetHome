using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Infrastructure.Database.Read.Configuration;
internal class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        //id
        builder.HasKey(x => x.Id);
        builder.Property(i => i.Id) 
            .IsRequired()
            .HasColumnName("id");

        //name
        builder.Property(i => i.Name) 
           .IsRequired()
           .HasColumnName("name");
    }
}
