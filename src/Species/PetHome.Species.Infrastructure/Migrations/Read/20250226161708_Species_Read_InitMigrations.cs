using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Species.Infrastructure.Migrations.Read
{
    /// <inheritdoc />
    public partial class Species_Read_InitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "specieses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specieses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_specieses_species_id",
                        column: x => x.species_id,
                        principalTable: "specieses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                table: "breeds",
                column: "species_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "specieses");
        }
    }
}
