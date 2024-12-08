using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_specieses_species_id",
                table: "breeds");

            migrationBuilder.DropIndex(
                name: "ix_breeds_species_id",
                table: "breeds");

            migrationBuilder.DropColumn(
                name: "species_id1",
                table: "breeds");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "breeds",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                table: "breeds",
                column: "species_id");

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

            migrationBuilder.DropIndex(
                name: "ix_breeds_species_id",
                table: "breeds");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "breeds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "species_id1",
                table: "breeds",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                table: "breeds",
                column: "species_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_specieses_species_id",
                table: "breeds",
                column: "species_id1",
                principalTable: "specieses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
