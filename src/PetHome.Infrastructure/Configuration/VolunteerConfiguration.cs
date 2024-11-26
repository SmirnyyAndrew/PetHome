using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.Shared;
using PetHome.Domain.VolunteerEntity;
using System.Reflection.Metadata;

namespace PetHome.Infrastructure.Configuration
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            //id
            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => VolunteerId.Create(value))
                .IsRequired()
                .HasColumnName("id");

            //fullname
            builder.ComplexProperty(f => f.FullName, tb =>
            {
                tb.Property(t => t.FirstName)
                .HasMaxLength(Constants.MAX_NAME_LENGHT)
                .IsRequired()
                .HasColumnName("first_name");

                tb.Property(t => t.LastName)
                .HasMaxLength(Constants.MAX_NAME_LENGHT)
                .IsRequired()
                .HasColumnName("last_name");
            });

            //email
            builder.Property(e => e.Email)
                .HasColumnName("email");

            //desc
            builder.Property(d => d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .HasColumnName("description");

            //pets
            builder.HasMany(m => m.PetList)
                .WithOne()
                .HasForeignKey("volunteer_id")
                .OnDelete(DeleteBehavior.NoAction);

            //phonenumber
            builder.Property(p => p.PhoneNumber)
                .HasConversion(
                    num => num.Value,
                    value => PhoneNumber.Create(value).Value)
                .HasColumnName("phone_number");

            //social networks
            builder.HasMany(s => s.SocialNetworkList);

            //requisites
            builder.ComplexProperty(req => req.Requisites, tb =>
            {
                tb.Property(n => n.Name)
                .IsRequired()
                .HasColumnName("requisit_name");

                tb.Property(d => d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .IsRequired()
                .HasColumnName("requisit_description");

                tb.Property(m => m.PaymentMethod)
                .IsRequired()
                .HasColumnName("requisit_payment_method");
            });




        }
    }
}
