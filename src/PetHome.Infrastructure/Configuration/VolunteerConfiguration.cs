using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.Shared;
using PetHome.Domain.VolunteerEntity;
using System.Text.Json;

namespace PetHome.Infrastructure.Configuration
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            //id
            builder.HasKey(x => x.Id); 
            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => VolunteerId.Create(value))
                .IsRequired()
                .HasColumnName("id");

            //fullname
            builder.ComplexProperty(f => f.FullName, tb =>
            {
                tb.Property(f => f.FirstName)
                    .IsRequired()
                    .HasColumnName("f_name");

                tb.Property(f => f.LastName)
                    .IsRequired()
                    .HasColumnName("l_name");
            }); 
              
            //email
            builder.Property(e => e.Email)
                .IsRequired(false)
                .HasColumnName("email");

            //desc
            builder.Property(d => d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .IsRequired()
                .HasColumnName("description");

             //StartVolunteeringDate
             builder.Property(s => s.StartVolunteeringDate)
                 .HasConversion(
                     d => d.ToShortDateString(),
                     d => DateOnly.Parse(d))
                 .IsRequired()
                 .HasColumnName("start_volunteering_date");

            //pets
            builder.HasMany(m => m.PetList)
                .WithOne()
                .HasForeignKey("volunteer_id")
                .OnDelete(DeleteBehavior.Cascade);

            //phonenumbers
            builder.OwnsOne(p => p.PhoneNumberDetails, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Values); ;
            });
             
            //social networks
            builder.OwnsOne(s => s.SocialNetworkDetails, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Values);
            });
             
            //requisites
            builder.OwnsOne(r => r.RequisitesDetails, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Values);
            }); 
        }
    }
}
