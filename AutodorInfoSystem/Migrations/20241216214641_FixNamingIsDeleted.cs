using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixNamingIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "workers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "materials",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "equipment",
                newName: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "workers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "materials",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "equipment",
                newName: "IsDeleted");
        }
    }
}
