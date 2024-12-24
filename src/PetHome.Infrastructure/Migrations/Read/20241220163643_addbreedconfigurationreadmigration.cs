using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Infrastructure.Migrations.Read
{
    /// <inheritdoc />
    public partial class addbreedconfigurationreadmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breed_dto_specieses_species_id",
                table: "breed_dto");

            migrationBuilder.DropPrimaryKey(
                name: "pk_breed_dto",
                table: "breed_dto");

            migrationBuilder.RenameTable(
                name: "breed_dto",
                newName: "breeds");

            migrationBuilder.RenameIndex(
                name: "ix_breed_dto_species_id",
                table: "breeds",
                newName: "ix_breeds_species_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_breeds",
                table: "breeds",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_specieses_species_id",
                table: "breeds",
                column: "species_id",
                principalTable: "specieses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_specieses_species_id",
                table: "breeds");

            migrationBuilder.DropPrimaryKey(
                name: "pk_breeds",
                table: "breeds");

            migrationBuilder.RenameTable(
                name: "breeds",
                newName: "breed_dto");

            migrationBuilder.RenameIndex(
                name: "ix_breeds_species_id",
                table: "breed_dto",
                newName: "ix_breed_dto_species_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_breed_dto",
                table: "breed_dto",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_breed_dto_specieses_species_id",
                table: "breed_dto",
                column: "species_id",
                principalTable: "specieses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
