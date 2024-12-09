using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "cost",
                table: "workers_has_tasks",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "cost",
                table: "tasks",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "cost",
                table: "projects",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "cost",
                table: "materials_has_tasks",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "cost",
                table: "equipment_has_tasks",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "workers_has_tasks");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "materials_has_tasks");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "equipment_has_tasks");
        }
    }
}
