using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutodorInfoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER after_equipment_update
                AFTER UPDATE ON equipment
                FOR EACH ROW
                BEGIN
                    UPDATE equipment_has_tasks
                    SET cost = NEW.price * quantity
                    WHERE id_equipment = NEW.id_equipment;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_equipment_has_tasks_update
                BEFORE UPDATE ON equipment_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE equipment_price DECIMAL(9,2);

	                SELECT price INTO equipment_price
                    FROM equipment
                    WHERE id_equipment = NEW.id_equipment;
    
                    SET NEW.cost = equipment_price * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_equipment_has_tasks_update
                AFTER UPDATE ON equipment_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_equipment_has_tasks_insert
                BEFORE INSERT ON equipment_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE equipment_price DECIMAL(9,2);

	                SELECT price INTO equipment_price
                    FROM equipment
                    WHERE id_equipment = NEW.id_equipment;
    
                    SET NEW.cost = equipment_price * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_equipment_has_tasks_insert
                AFTER INSERT ON equipment_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_equipment_has_tasks_delete
                AFTER DELETE ON equipment_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = OLD.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = OLD.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_materials_update
                AFTER UPDATE ON materials
                FOR EACH ROW
                BEGIN
                    UPDATE materials_has_tasks
                    SET cost = NEW.price * quantity
                    WHERE id_material = NEW.id_material;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_materials_has_tasks_update
                BEFORE UPDATE ON materials_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE material_price DECIMAL(9,2);

	                SELECT price INTO material_price
                    FROM materials
                    WHERE id_material = NEW.id_material;

                    SET NEW.cost = material_price * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_materials_has_tasks_update
                AFTER UPDATE ON materials_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_materials_has_tasks_insert
                BEFORE INSERT ON materials_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE material_price DECIMAL(9,2);

	                SELECT price INTO material_price
                    FROM materials
                    WHERE id_material = NEW.id_material;
    
                    SET NEW.cost = material_price * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_materials_has_tasks_insert
                AFTER INSERT ON materials_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_materials_has_tasks_delete
                AFTER DELETE ON materials_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = OLD.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = OLD.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_workers_update
                AFTER UPDATE ON workers
                FOR EACH ROW
                BEGIN
                    UPDATE workers_has_tasks
                    SET cost = NEW.salary * quantity
                    WHERE id_worker = NEW.id_worker;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_workers_has_tasks_update
                BEFORE UPDATE ON workers_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE worker_salary DECIMAL(9,2);

	                SELECT salary INTO worker_salary
                    FROM workers
                    WHERE id_worker = NEW.id_worker;
    
                    SET NEW.cost = worker_salary * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_workers_has_tasks_update
                AFTER UPDATE ON workers_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER before_workers_has_tasks_insert
                BEFORE INSERT ON workers_has_tasks
                FOR EACH ROW
                BEGIN
	                DECLARE worker_salary DECIMAL(9,2);

	                SELECT salary INTO worker_salary
                    FROM workers
                    WHERE id_worker = NEW.id_worker;
    
                    SET NEW.cost = worker_salary * NEW.quantity;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_workers_has_tasks_insert
                AFTER INSERT ON workers_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = NEW.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = NEW.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = NEW.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_workers_has_tasks_delete
                AFTER DELETE ON workers_has_tasks
                FOR EACH ROW
                BEGIN
                    DECLARE task_cost DECIMAL(11,2);
                    DECLARE equipment_cost DECIMAL(11,2);
                    DECLARE materials_cost DECIMAL(11,2);
                    DECLARE workers_cost DECIMAL(11,2);

                    SELECT COALESCE(SUM(cost), 0) INTO equipment_cost
                    FROM equipment_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO materials_cost
                    FROM materials_has_tasks
                    WHERE id_task = OLD.id_task;

                    SELECT COALESCE(SUM(cost), 0) INTO workers_cost
                    FROM workers_has_tasks
                    WHERE id_task = OLD.id_task;
    
                    SET task_cost = equipment_cost + materials_cost + workers_cost;
    
                    UPDATE tasks
                    SET cost = task_cost
                    WHERE id_task = OLD.id_task;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_tasks_update
                AFTER UPDATE ON tasks
                FOR EACH ROW
                BEGIN
	                DECLARE task_cost DECIMAL(11,2);
    
                    SELECT SUM(cost) INTO task_cost
                    FROM tasks
                    WHERE id_project = NEW.id_project;
    
                    UPDATE projects
                    SET cost = task_cost
                    WHERE id_project = NEW.id_project;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_tasks_delete
                AFTER DELETE ON tasks
                FOR EACH ROW
                BEGIN
	                DECLARE task_cost DECIMAL(11,2);
    
                    SELECT SUM(cost) INTO task_cost
                    FROM tasks
                    WHERE id_project = OLD.id_project;
    
                    UPDATE projects
                    SET cost = task_cost
                    WHERE id_project = OLD.id_project;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER after_tasks_insert
                AFTER INSERT ON tasks
                FOR EACH ROW
                BEGIN
	                DECLARE task_cost DECIMAL(11,2);
    
                    SELECT SUM(cost) INTO task_cost
                    FROM tasks
                    WHERE id_project = NEW.id_project;
    
                    UPDATE projects
                    SET cost = task_cost
                    WHERE id_project = NEW.id_project;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_equipment_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_equipment_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_equipment_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_equipment_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_equipment_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_equipment_has_tasks_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_materials_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_materials_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_materials_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_materials_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_materials_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_materials_has_tasks_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_workers_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_workers_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_workers_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_workers_has_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_workers_has_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_workers_has_tasks_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_tasks_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_tasks_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS after_tasks_delete;");
        }
    }
}
