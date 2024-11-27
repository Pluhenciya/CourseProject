using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id_equipment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_equipment);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id_material = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    measurement_unit = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_material);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id_project = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_completed = table.Column<ulong>(type: "bit(1)", nullable: false, defaultValueSql: "b'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_project);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    login = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_user);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "workers",
                columns: table => new
                {
                    id_worker = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_worker);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id_task = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    projects_id_project = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_task);
                    table.ForeignKey(
                        name: "fk_tasks_projects1",
                        column: x => x.projects_id_project,
                        principalTable: "projects",
                        principalColumn: "id_project");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    users_id_user = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.users_id_user);
                    table.ForeignKey(
                        name: "fk_admins_users1",
                        column: x => x.users_id_user,
                        principalTable: "users",
                        principalColumn: "id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "projecters",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false),
                    surname = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    patronymic = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_user);
                    table.ForeignKey(
                        name: "fk_projecters_users",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "equipment_has_tasks",
                columns: table => new
                {
                    equipment_id_equipment = table.Column<int>(type: "int", nullable: false),
                    tasks_id_task = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.equipment_id_equipment, x.tasks_id_task })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_equipment_has_tasks_equipment1",
                        column: x => x.equipment_id_equipment,
                        principalTable: "equipment",
                        principalColumn: "id_equipment");
                    table.ForeignKey(
                        name: "fk_equipment_has_tasks_tasks1",
                        column: x => x.tasks_id_task,
                        principalTable: "tasks",
                        principalColumn: "id_task");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "materials_has_tasks",
                columns: table => new
                {
                    materials_id_material = table.Column<int>(type: "int", nullable: false),
                    tasks_id_task = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.materials_id_material, x.tasks_id_task })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_materials_has_tasks_materials1",
                        column: x => x.materials_id_material,
                        principalTable: "materials",
                        principalColumn: "id_material");
                    table.ForeignKey(
                        name: "fk_materials_has_tasks_tasks1",
                        column: x => x.tasks_id_task,
                        principalTable: "tasks",
                        principalColumn: "id_task");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "workers_has_tasks",
                columns: table => new
                {
                    workers_id_worker = table.Column<int>(type: "int", nullable: false),
                    tasks_id_task = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.workers_id_worker, x.tasks_id_task })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_workers_has_tasks_tasks1",
                        column: x => x.tasks_id_task,
                        principalTable: "tasks",
                        principalColumn: "id_task");
                    table.ForeignKey(
                        name: "fk_workers_has_tasks_workers1",
                        column: x => x.workers_id_worker,
                        principalTable: "workers",
                        principalColumn: "id_worker");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "projecters_has_projects",
                columns: table => new
                {
                    projecters_id_user = table.Column<int>(type: "int", nullable: false),
                    projects_id_project = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.projecters_id_user, x.projects_id_project })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_projecters_has_projects_projecters1",
                        column: x => x.projecters_id_user,
                        principalTable: "projecters",
                        principalColumn: "id_user");
                    table.ForeignKey(
                        name: "fk_projecters_has_projects_projects1",
                        column: x => x.projects_id_project,
                        principalTable: "projects",
                        principalColumn: "id_project");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_equipment_has_tasks_equipment1_idx",
                table: "equipment_has_tasks",
                column: "equipment_id_equipment");

            migrationBuilder.CreateIndex(
                name: "fk_equipment_has_tasks_tasks1_idx",
                table: "equipment_has_tasks",
                column: "tasks_id_task");

            migrationBuilder.CreateIndex(
                name: "fk_materials_has_tasks_materials1_idx",
                table: "materials_has_tasks",
                column: "materials_id_material");

            migrationBuilder.CreateIndex(
                name: "fk_materials_has_tasks_tasks1_idx",
                table: "materials_has_tasks",
                column: "tasks_id_task");

            migrationBuilder.CreateIndex(
                name: "fk_projecters_has_projects_projecters1_idx",
                table: "projecters_has_projects",
                column: "projecters_id_user");

            migrationBuilder.CreateIndex(
                name: "fk_projecters_has_projects_projects1_idx",
                table: "projecters_has_projects",
                column: "projects_id_project");

            migrationBuilder.CreateIndex(
                name: "fk_tasks_projects1_idx",
                table: "tasks",
                column: "projects_id_project");

            migrationBuilder.CreateIndex(
                name: "login_UNIQUE",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_workers_has_tasks_tasks1_idx",
                table: "workers_has_tasks",
                column: "tasks_id_task");

            migrationBuilder.CreateIndex(
                name: "fk_workers_has_tasks_workers1_idx",
                table: "workers_has_tasks",
                column: "workers_id_worker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "equipment_has_tasks");

            migrationBuilder.DropTable(
                name: "materials_has_tasks");

            migrationBuilder.DropTable(
                name: "projecters_has_projects");

            migrationBuilder.DropTable(
                name: "workers_has_tasks");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "projecters");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "workers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
