using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Discussions.Infrastructure.Migrations.Write
{
    /// <inheritdoc />
    public partial class Discussions_Write_InitMigrations1223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Discussions");

            migrationBuilder.CreateTable(
                name: "relations",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discussions",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    relation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discussions", x => x.id);
                    table.ForeignKey(
                        name: "fk_discussions_relations_relation_id",
                        column: x => x.relation_id,
                        principalSchema: "Discussions",
                        principalTable: "relations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_discussions_discussion_id",
                        column: x => x.discussion_id,
                        principalSchema: "Discussions",
                        principalTable: "discussions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_discussions_relation_id",
                schema: "Discussions",
                table: "discussions",
                column: "relation_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_discussion_id",
                schema: "Discussions",
                table: "messages",
                column: "discussion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "discussions",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "relations",
                schema: "Discussions");
        }
    }
}
