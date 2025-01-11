using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Aggregates.User.Accounts;
using PetHome.Core.ValueObjects;
using System.Text.Json;

namespace PetHome.Accounts.Infrastructure.Database.Configuration.Users;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.RoleId)
            .HasConversion(
                i => i.Value,
                value => RoleId.Create(value).Value)
            .IsRequired()
            .HasColumnName("role_id");

        builder.Property(r => r.BirthDate)
            .HasConversion(
                d => d.Value,
                value => Date.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("birth_date");

        builder.Property(d => d.IsDeleted)
            .HasColumnName("is_deleted");

        builder.Property(d => d.DeletionDate)
            .HasColumnName("deletion_date");

        builder.Property(s => s.SocialNetworks)
            .HasConversion(
                 u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                 json => JsonSerializer.Deserialize<IReadOnlyList<SocialNetwork>>(json, JsonSerializerOptions.Default))
            .HasColumnName("social_networks");

        builder.Property(s => s.Medias)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<Media>>(json, JsonSerializerOptions.Default))
            .HasColumnName("medias");

        builder.Property(s => s.PhoneNumbers)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<PhoneNumber>>(json, JsonSerializerOptions.Default))
            .HasColumnName("phone_number");

        builder.HasOne(d => d.Role)
            .WithMany();


        builder.HasOne(u => u.Admin)
                .WithOne(u => u.User)
                .HasPrincipalKey<AdminAccount>(d => d.UserId)
                .IsRequired(false);

        builder.HasOne(u => u.Participant)
                .WithOne(u => u.User)
                .HasPrincipalKey<ParticipantAccount>(d => d.UserId)
                .IsRequired(false);

        builder.HasOne(u => u.Volunteer)
                .WithOne(u => u.User)
                .HasPrincipalKey<VolunteerAccount>(d => d.UserId)
                .IsRequired(false);
    }
}
