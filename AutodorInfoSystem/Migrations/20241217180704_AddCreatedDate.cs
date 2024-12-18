using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "workers",
                type: "datetime",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "projects",
                type: "datetime",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "materials",
                type: "datetime",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "equipment",
                type: "datetime",
                nullable: false,
                defaultValueSql: "NOW()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_date",
                table: "workers");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "materials");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "equipment");
        }
    }
}
