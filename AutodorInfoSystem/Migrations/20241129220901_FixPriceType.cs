using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixPriceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "salary",
                table: "workers",
                type: "decimal(9,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "materials",
                type: "decimal(9,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "equipment",
                type: "decimal(9,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "salary",
                table: "workers",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "decimal(9,2)");

            migrationBuilder.AlterColumn<int>(
                name: "price",
                table: "materials",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "decimal(9,2)");

            migrationBuilder.AlterColumn<int>(
                name: "price",
                table: "equipment",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "decimal(9,2)");
        }
    }
}
