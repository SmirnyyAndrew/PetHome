using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetEntity;
using PetHome.Domain.Shared;

namespace PetHome.Infrastructure.Configuration
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            //id
            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => PetId.Create(value))
                .IsRequired()
                .HasColumnName("id");

            //name
            builder.Property(i => i.Name)
                .HasMaxLength(Constants.MAX_NAME_LENGHT)
                .IsRequired()
                .HasColumnName("name");

            //speacies
            builder.Property(s=>s.Species.Id)
                .HasColumnName("species_id");

            //desc
            builder.Property(d=>d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .IsRequired()
                .HasColumnName("description");

            //breed
            builder.Property(x => x.Breed.Id)
                .IsRequired()
                .HasColumnName("breed_id");

            //color
            builder.Property(c => c.Color)
                .HasConversion(color => color.Value, value => Color.Create(value).Value);

            //shelter
            builder.Property(s => s.Address.Id)
                .IsRequired()
                .HasColumnName("shelter_address_id");

            //weight
            builder.Property(w => w.Weight)
                .HasMaxLength(Constants.MAX_WEIGHT)
                .IsRequired()
                .HasColumnName("weight");

            //is castrated
            builder.Property(c => c.IsCastrated)
                .IsRequired()
                .HasColumnName("is_castrated");

            //bith date
            builder.Property(d => d.BirthDate)
                .HasConversion(date => date.Value, value => VO_Date.Create(value).Value)
                .HasColumnName("birth_date");

            //is vaccinated
            builder.Property(c => c.IsVaccinated)
                .IsRequired()
                .HasColumnName("is_vaccinated");

            //status
            builder.Property(s => s.Status)
                .IsRequired()
                .HasColumnName("status");

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

            //create date
            builder.Property(d => d.ProfileCreateDate)
                .HasConversion(date => date.Value, value => VO_Date.Create(value).Value)
                .HasColumnName("profile_create_date");

            //volunteer id
            builder.Property(v => v.Volunteer.Id)
                .IsRequired()
                .HasColumnName("volunteer_id");

        }
    }
}
