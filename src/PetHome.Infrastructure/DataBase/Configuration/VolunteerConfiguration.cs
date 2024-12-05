using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared;

namespace PetHome.Infrastructure.DataBase.Configuration;
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
                .HasColumnName("first_name");

            tb.Property(f => f.LastName)
                .IsRequired()
                .HasColumnName("last_name");
        });

        //email 
        builder.Property(i => i.Email)
            .HasConversion(
                id => id.Value,
                value => Email.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("email");

        //desc
        builder.Property(d=>d.Description)
            .HasConversion(
                desc=>desc.Value,
                value=>Description.Create(value).Value)
            .IsRequired()
            .HasColumnName("description");
                
        //StartVolunteeringDate
        builder.Property(i => i.StartVolunteeringDate)
            .HasConversion(
                d => d.Value,
                value => Date.Create(value).Value)
            .IsRequired()
            .HasColumnName("start_volunteering_date");

        //pets
        builder.HasMany(m => m.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        //phonenumbers
        builder.OwnsOne(p => p.PhoneNumberDetails, d =>
        {
            d.ToJson();
            d.OwnsMany(d => d.Values, pb =>
            {
                pb.Property(p => p.Value)
                .IsRequired();
            });
        });

        //social networks
        builder.OwnsOne(s => s.SocialNetworkDetails, d =>
        {
            d.ToJson();
            d.OwnsMany(d => d.Values, sb =>
            {
                sb.Property(p => p.Value)
                .IsRequired();
            });
        });

        //requisites
        builder.OwnsOne(r => r.RequisitesDetails, d =>
        {
            d.ToJson();
            d.OwnsMany(d => d.Values, rb =>
            {
                rb.Property(r => r.Name)
                .IsRequired();

                rb.Property(r => r.Description)
                .IsRequired();

                rb.Property(r => r.PaymentMethod)
                .IsRequired();
            });
        });
    }
}
