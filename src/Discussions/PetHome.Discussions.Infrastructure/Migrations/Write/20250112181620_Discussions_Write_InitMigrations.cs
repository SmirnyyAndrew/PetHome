using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Discussions.Infrastructure.Migrations.Write
{
    /// <inheritdoc />
    public partial class Discussions_Write_InitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Discussions");

            migrationBuilder.CreateTable(
                name: "admin_accounts",
                schema: "Discussions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false),
                    soft_deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_accounts", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "participant_accounts",
                schema: "Discussions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    favorite_pets = table.Column<string>(type: "text", nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false),
                    soft_deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_participant_accounts", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_sessions",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token = table.Column<Guid>(type: "uuid", nullable: false),
                    jti = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expired_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_sessions", x => x.id);
                });

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
                name: "role_permission",
                schema: "Discussions",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permission", x => new { x.role_id, x.permission_id });
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    normalized_name = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volunteer_accounts",
                schema: "Discussions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_volunteering_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    requisites = table.Column<string>(type: "text", nullable: true),
                    certificates = table.Column<string>(type: "text", nullable: true),
                    pets = table.Column<string>(type: "text", nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false),
                    soft_deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ak_volunteer_account_user_id", x => x.user_id);
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
                name: "permission_role",
                schema: "Discussions",
                columns: table => new
                {
                    permissions_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_role", x => new { x.permissions_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_permission_role_permissions_permissions_id",
                        column: x => x.permissions_id,
                        principalSchema: "Discussions",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permission_role_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "Discussions",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "Discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    social_networks = table.Column<string>(type: "text", nullable: true),
                    medias = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    admin_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    participant_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    normalized_user_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    normalized_email = table.Column<string>(type: "text", nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number1 = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_admin_accounts_admin_user_id",
                        column: x => x.admin_user_id,
                        principalSchema: "Discussions",
                        principalTable: "admin_accounts",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_users_participant_accounts_participant_user_id",
                        column: x => x.participant_user_id,
                        principalSchema: "Discussions",
                        principalTable: "participant_accounts",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_users_roles_role_id1",
                        column: x => x.role_id1,
                        principalSchema: "Discussions",
                        principalTable: "roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_volunteer_account_volunteer_user_id",
                        column: x => x.volunteer_user_id,
                        principalSchema: "Discussions",
                        principalTable: "volunteer_accounts",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "discussion_user",
                schema: "Discussions",
                columns: table => new
                {
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discussion_user", x => new { x.discussion_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_discussion_user_discussions_discussion_id",
                        column: x => x.discussion_id,
                        principalSchema: "Discussions",
                        principalTable: "discussions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_discussion_user_user_users_id",
                        column: x => x.users_id,
                        principalSchema: "Discussions",
                        principalTable: "users",
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
                    user_id1 = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "fk_messages_user_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "Discussions",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_discussion_user_users_id",
                schema: "Discussions",
                table: "discussion_user",
                column: "users_id");

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

            migrationBuilder.CreateIndex(
                name: "ix_messages_user_id1",
                schema: "Discussions",
                table: "messages",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_role_id",
                schema: "Discussions",
                table: "permission_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_admin_user_id",
                schema: "Discussions",
                table: "users",
                column: "admin_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_participant_user_id",
                schema: "Discussions",
                table: "users",
                column: "participant_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id1",
                schema: "Discussions",
                table: "users",
                column: "role_id1");

            migrationBuilder.CreateIndex(
                name: "ix_users_volunteer_user_id",
                schema: "Discussions",
                table: "users",
                column: "volunteer_user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discussion_user",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "messages",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "permission_role",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "refresh_sessions",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "role_permission",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "discussions",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "users",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "relations",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "admin_accounts",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "participant_accounts",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "Discussions");

            migrationBuilder.DropTable(
                name: "volunteer_accounts",
                schema: "Discussions");
        }
    }
}
