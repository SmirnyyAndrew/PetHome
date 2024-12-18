using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Infrastructure.Migrations.Read
{
    /// <inheritdoc />
    public partial class ChangeVolunteerDtoReadMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "soft_deleted_date",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "volunteers",
                newName: "last_name");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "volunteers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "volunteers");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "volunteers",
                newName: "full_name");

            migrationBuilder.AddColumn<DateTime>(
                name: "soft_deleted_date",
                table: "Pets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
