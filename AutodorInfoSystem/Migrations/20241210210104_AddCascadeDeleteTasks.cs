using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipment_has_tasks_tasks1",
                table: "equipment_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_has_tasks_tasks1",
                table: "materials_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_workers_has_tasks_workers1",
                table: "workers_has_tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_equipment_has_tasks_tasks1",
                table: "equipment_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_materials_has_tasks_tasks1",
                table: "materials_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_workers_has_tasks_workers1",
                table: "workers_has_tasks",
                column: "id_worker",
                principalTable: "workers",
                principalColumn: "id_worker",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipment_has_tasks_tasks1",
                table: "equipment_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_has_tasks_tasks1",
                table: "materials_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_workers_has_tasks_workers1",
                table: "workers_has_tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_equipment_has_tasks_tasks1",
                table: "equipment_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task");

            migrationBuilder.AddForeignKey(
                name: "fk_materials_has_tasks_tasks1",
                table: "materials_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task");

            migrationBuilder.AddForeignKey(
                name: "fk_workers_has_tasks_workers1",
                table: "workers_has_tasks",
                column: "id_worker",
                principalTable: "workers",
                principalColumn: "id_worker");
        }
    }
}
