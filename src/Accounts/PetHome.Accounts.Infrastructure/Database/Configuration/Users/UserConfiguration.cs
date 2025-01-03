using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Core.ValueObjects;
using System.Text.Json;

namespace PetHome.Accounts.Infrastructure.Database.Configuration.Users;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(r => r.RoleId)
            .HasConversion(
                i => i.Value,
                value => RoleId.Create(value).Value)
            .IsRequired()
            .HasColumnName("role_id");

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
    }
}
