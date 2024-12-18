using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetHome.Infrastructure.DataBase.Read.Configuration;
public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        //id
        builder.HasKey(x => x.Id);

        //fullname
        builder.Property(f => f.FullName)
                .IsRequired()
                .HasColumnName("full_name");


        //email 
        builder.Property(i => i.Email)
            .IsRequired(false)
            .HasColumnName("email");

        //desc
        builder.Property(d => d.Description)
            .IsRequired()
            .HasColumnName("description");

        //StartVolunteeringDate
        builder.Property(i => i.StartVolunteeringDate)
            .IsRequired()
            .HasColumnName("start_volunteering_date");

        ////phone numbers
        //builder.OwnsMany(p => p.PhoneNumbers);

        ////Is soft deleted
        //builder.Property<bool>("_isDeleted")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field)
        //    .HasColumnName("is_deleted");

        ////has been deleted date
        //builder.Property<DateTime>("DeletionDate")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field)
        //    .HasColumnName("soft_deleted_date");
    }
}
