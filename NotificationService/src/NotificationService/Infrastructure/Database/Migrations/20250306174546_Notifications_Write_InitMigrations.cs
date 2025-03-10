using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Notifications_Write_InitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "UserNotificationSettings",
                schema: "Notifications",
                columns: table => new
                {
                    user_notification_settings_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_email_send = table.Column<bool>(type: "boolean", nullable: true),
                    is_web_send = table.Column<bool>(type: "boolean", nullable: true),
                    telegram_settings = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_id", x => x.user_notification_settings_user_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotificationSettings",
                schema: "Notifications");
        }
    }
}
