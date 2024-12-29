using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Infrastructure.Migrations.Read
{
    /// <inheritdoc />
    public partial class addidtospeciesdtoreadmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "specieses",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "specieses",
                newName: "id");
        }
    }
}
