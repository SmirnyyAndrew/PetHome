using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Accounts.Domain.Aggregates;

namespace PetHome.Accounts.Infrastructure.Database.Configuration.Permissions;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(i => i.Id);
         
        builder.Property(i => i.Name)
            .IsRequired()
            .HasColumnName("name");

        builder.HasMany(p => p.Permissions)
            .WithMany();
    }
}
