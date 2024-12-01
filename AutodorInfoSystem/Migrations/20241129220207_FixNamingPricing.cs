using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixNamingPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "workers",
                newName: "salary");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "materials",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "equipment",
                newName: "price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "salary",
                table: "workers",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "materials",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "equipment",
                newName: "Price");
        }
    }
}
