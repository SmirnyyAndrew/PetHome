using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain;

namespace NotificationService.Infrastructure.Database.Configuration;

public class UserNotificationSettingsConfiguration
    : IEntityTypeConfiguration<UserNotificationSettings>
{
    public void Configure(EntityTypeBuilder<UserNotificationSettings> builder)
    {
        builder.ToTable("UserNotificationSettings");

        builder.HasKey(n => n.UserId)
            .HasName("user_id");

        builder.Property(n => n.IsEmailSend)
            .IsRequired(false)
            .HasColumnName("is_email_send");

        builder.OwnsOne(n => n.TelegramSettings, ts =>
        {
            ts.ToJson("telegram_settings");

            ts.Property(s => s.UserId)
            .IsRequired()
            .HasColumnName("user_id");

            ts.Property(s => s.ChatId)
            .IsRequired()
            .HasColumnName("chat_id");
        });

        builder.Property(n => n.IsWebSend)
            .IsRequired(false)
            .HasColumnName("is_web_send");
    }
}
