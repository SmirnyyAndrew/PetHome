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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_email_send = table.Column<bool>(type: "boolean", nullable: true),
                    is_telegram_send = table.Column<bool>(type: "boolean", nullable: true),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: true),
                    is_web_send = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_id", x => x.user_id);
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
