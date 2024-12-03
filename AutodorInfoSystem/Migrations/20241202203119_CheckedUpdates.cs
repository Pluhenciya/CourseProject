using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class CheckedUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tasks_id_task",
                table: "workers_has_tasks",
                newName: "id_task");

            migrationBuilder.RenameColumn(
                name: "workers_id_worker",
                table: "workers_has_tasks",
                newName: "id_worker");

            migrationBuilder.RenameColumn(
                name: "tasks_id_task",
                table: "materials_has_tasks",
                newName: "id_task");

            migrationBuilder.RenameColumn(
                name: "materials_id_material",
                table: "materials_has_tasks",
                newName: "id_material");

            migrationBuilder.RenameColumn(
                name: "tasks_id_task",
                table: "equipment_has_tasks",
                newName: "id_task");

            migrationBuilder.RenameColumn(
                name: "equipment_id_equipment",
                table: "equipment_has_tasks",
                newName: "id_equipment");

            migrationBuilder.AlterColumn<double>(
                name: "salary",
                table: "workers",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "materials",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "equipment",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_task",
                table: "workers_has_tasks",
                newName: "tasks_id_task");

            migrationBuilder.RenameColumn(
                name: "id_worker",
                table: "workers_has_tasks",
                newName: "workers_id_worker");

            migrationBuilder.RenameColumn(
                name: "id_task",
                table: "materials_has_tasks",
                newName: "tasks_id_task");

            migrationBuilder.RenameColumn(
                name: "id_material",
                table: "materials_has_tasks",
                newName: "materials_id_material");

            migrationBuilder.RenameColumn(
                name: "id_task",
                table: "equipment_has_tasks",
                newName: "tasks_id_task");

            migrationBuilder.RenameColumn(
                name: "id_equipment",
                table: "equipment_has_tasks",
                newName: "equipment_id_equipment");

            migrationBuilder.AlterColumn<float>(
                name: "salary",
                table: "workers",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "materials",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "equipment",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
