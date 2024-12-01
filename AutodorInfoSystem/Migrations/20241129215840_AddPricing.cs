using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "workers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "materials");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "equipment");
        }
    }
}
