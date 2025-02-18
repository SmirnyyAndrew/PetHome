using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Accounts_Add_MediasUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "medias",
                schema: "Account",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                schema: "Account",
                table: "users",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                schema: "Account",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "medias",
                schema: "Account",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
