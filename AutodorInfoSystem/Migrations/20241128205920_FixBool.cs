using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "workers_has_tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "materials_has_tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "'1'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "workers_has_tasks",
                type: "int",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "materials_has_tasks",
                type: "int",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
