using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.Response.RefreshToken;
using PetHome.Core.ValueObjects.User;

namespace AccountService.Infrastructure.Database.Configuration.Tokens;
public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.ToTable("refresh_sessions");

        builder.HasKey(t => t.Id);

        builder.Property(r => r.RefreshToken)
            .IsRequired()
            .HasColumnName("refresh_token");

        builder.Property(j => j.JTI)
            .IsRequired()
            .HasColumnName("jti");

        builder.Property(u => u.UserId) 
            .IsRequired()
            .HasColumnName("user_id"); 

        builder.Property(e => e.ExpiredIn)
            .IsRequired()
            .HasColumnName("expired_in");

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
    }
}
