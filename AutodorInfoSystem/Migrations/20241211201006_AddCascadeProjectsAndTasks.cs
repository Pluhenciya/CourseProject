using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeProjectsAndTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_projects1",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_projects1",
                table: "tasks",
                column: "id_project",
                principalTable: "projects",
                principalColumn: "id_project",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_projects1",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_projects1",
                table: "tasks",
                column: "id_project",
                principalTable: "projects",
                principalColumn: "id_project");
        }
    }
}
