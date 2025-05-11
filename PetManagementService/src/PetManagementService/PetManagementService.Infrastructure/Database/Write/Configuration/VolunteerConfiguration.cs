using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetHome.SharedKernel.ValueObjects.User;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Infrastructure.Database.Write.Configuration;
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
                value => VolunteerId.Create(value).Value)
            .IsRequired()
            .HasColumnName("id");

        //user id 
        builder.Property(i => i.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value).Value)
            .IsRequired()
            .HasColumnName("user_id");

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
        builder.Property(d => d.Description)
            .HasConversion(
                desc => desc.Value,
                value => Description.Create(value).Value)
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

        //phone numbers
        builder.OwnsOne(p => p.PhoneNumbers, d =>
        {
            d.ToJson("phone_numbers");
            d.OwnsMany(d => d.Values, pb =>
            {
                pb.Property(p => p.Value)
                .IsRequired();
            });
        });

        //social networks
        builder.OwnsOne(s => s.SocialNetworks, d =>
        {
            d.ToJson("social_networks");
            d.OwnsMany(d => d.Values, sb =>
            {
                sb.Property(p => p.Value)
                .IsRequired();
            });
        });

        //requisites
        builder.OwnsOne(r => r.Requisites, d =>
        {
            d.ToJson("requisites");
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

        //Is soft deleted
        builder.Property(d => d.IsDeleted) 
            .HasColumnName("is_deleted");

        //has been deleted date
        builder.Property(d => d.DeletionDate) 
            .HasColumnName("soft_deleted_date");
    }
}
