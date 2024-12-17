using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForFixBags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipment_has_tasks_equipment1",
                table: "equipment_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_has_tasks_materials1",
                table: "materials_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_workers_has_tasks_tasks1",
                table: "workers_has_tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_equipment_has_tasks_equipment1",
                table: "equipment_has_tasks",
                column: "id_equipment",
                principalTable: "equipment",
                principalColumn: "id_equipment",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_materials_has_tasks_materials1",
                table: "materials_has_tasks",
                column: "id_material",
                principalTable: "materials",
                principalColumn: "id_material",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_workers_has_tasks_tasks1",
                table: "workers_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipment_has_tasks_equipment1",
                table: "equipment_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_has_tasks_materials1",
                table: "materials_has_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_workers_has_tasks_tasks1",
                table: "workers_has_tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_equipment_has_tasks_equipment1",
                table: "equipment_has_tasks",
                column: "id_equipment",
                principalTable: "equipment",
                principalColumn: "id_equipment");

            migrationBuilder.AddForeignKey(
                name: "fk_materials_has_tasks_materials1",
                table: "materials_has_tasks",
                column: "id_material",
                principalTable: "materials",
                principalColumn: "id_material");

            migrationBuilder.AddForeignKey(
                name: "fk_workers_has_tasks_tasks1",
                table: "workers_has_tasks",
                column: "id_task",
                principalTable: "tasks",
                principalColumn: "id_task");
        }
    }
}
