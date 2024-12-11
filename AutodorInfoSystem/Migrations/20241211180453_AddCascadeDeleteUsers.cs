using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admins_users1",
                table: "admins");

            migrationBuilder.DropForeignKey(
                name: "fk_projecters_users",
                table: "projecters");

            migrationBuilder.AddForeignKey(
                name: "fk_admins_users1",
                table: "admins",
                column: "users_id_user",
                principalTable: "users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_projecters_users",
                table: "projecters",
                column: "id_user",
                principalTable: "users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admins_users1",
                table: "admins");

            migrationBuilder.DropForeignKey(
                name: "fk_projecters_users",
                table: "projecters");

            migrationBuilder.AddForeignKey(
                name: "fk_admins_users1",
                table: "admins",
                column: "users_id_user",
                principalTable: "users",
                principalColumn: "id_user");

            migrationBuilder.AddForeignKey(
                name: "fk_projecters_users",
                table: "projecters",
                column: "id_user",
                principalTable: "users",
                principalColumn: "id_user");
        }
    }
}
