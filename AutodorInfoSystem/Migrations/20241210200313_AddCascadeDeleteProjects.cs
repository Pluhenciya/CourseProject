using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projecters_has_projects_projects1",
                table: "projecters_has_projects");

            migrationBuilder.AddForeignKey(
                name: "fk_projecters_has_projects_projects1",
                table: "projecters_has_projects",
                column: "projects_id_project",
                principalTable: "projects",
                principalColumn: "id_project",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projecters_has_projects_projects1",
                table: "projecters_has_projects");

            migrationBuilder.AddForeignKey(
                name: "fk_projecters_has_projects_projects1",
                table: "projecters_has_projects",
                column: "projects_id_project",
                principalTable: "projects",
                principalColumn: "id_project");
        }
    }
}
