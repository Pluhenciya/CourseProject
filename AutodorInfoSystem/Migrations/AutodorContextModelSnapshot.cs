﻿// <auto-generated />
using AutodorInfoSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutodorInfoSystem.Migrations
{
    [DbContext(typeof(AutodorContext))]
    partial class AutodorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AutodorInfoSystem.Models.Admin", b =>
                {
                    b.Property<int>("UsersIdUser")
                        .HasColumnType("int")
                        .HasColumnName("users_id_user");

                    b.HasKey("UsersIdUser")
                        .HasName("PRIMARY");

                    b.ToTable("admins", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Equipment", b =>
                {
                    b.Property<int>("IdEquipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_equipment");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdEquipment"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("price");

                    b.HasKey("IdEquipment")
                        .HasName("PRIMARY");

                    b.ToTable("equipment", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.EquipmentHasTask", b =>
                {
                    b.Property<int>("IdEquipment")
                        .HasColumnType("int")
                        .HasColumnName("id_equipment");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    b.Property<double>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("cost");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("'1'");

                    b.HasKey("IdEquipment", "IdTask")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "IdEquipment" }, "fk_equipment_has_tasks_equipment1_idx");

                    b.HasIndex(new[] { "IdTask" }, "fk_equipment_has_tasks_tasks1_idx");

                    b.ToTable("equipment_has_tasks", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Material", b =>
                {
                    b.Property<int>("IdMaterial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_material");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdMaterial"));

                    b.Property<string>("MeasurementUnit")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)")
                        .HasColumnName("measurement_unit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("price");

                    b.HasKey("IdMaterial")
                        .HasName("PRIMARY");

                    b.ToTable("materials", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.MaterialsHasTask", b =>
                {
                    b.Property<int>("IdMaterial")
                        .HasColumnType("int")
                        .HasColumnName("id_material");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    b.Property<double>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("cost");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("IdMaterial", "IdTask")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "IdMaterial" }, "fk_materials_has_tasks_materials1_idx");

                    b.HasIndex(new[] { "IdTask" }, "fk_materials_has_tasks_tasks1_idx");

                    b.ToTable("materials_has_tasks", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Project", b =>
                {
                    b.Property<int>("IdProject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdProject"));

                    b.Property<double>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("cost");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<ulong>("IsCompleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit(1)")
                        .HasColumnName("is_completed")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.HasKey("IdProject")
                        .HasName("PRIMARY");

                    b.ToTable("projects", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Projecter", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("int")
                        .HasColumnName("id_user");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("patronymic");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("surname");

                    b.HasKey("IdUser")
                        .HasName("PRIMARY");

                    b.ToTable("projecters", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Task", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdTask"));

                    b.Property<double>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("cost");

                    b.Property<string>("Description")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("description");

                    b.Property<int>("IdProject")
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.HasKey("IdTask")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdProject" }, "fk_tasks_projects1_idx");

                    b.ToTable("tasks", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_user");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.HasKey("IdUser")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Login" }, "login_UNIQUE")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Worker", b =>
                {
                    b.Property<int>("IdWorker")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_worker");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdWorker"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<double>("Salary")
                        .HasColumnType("double")
                        .HasColumnName("salary");

                    b.HasKey("IdWorker")
                        .HasName("PRIMARY");

                    b.ToTable("workers", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.WorkersHasTask", b =>
                {
                    b.Property<int>("IdWorker")
                        .HasColumnType("int")
                        .HasColumnName("id_worker");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    b.Property<double>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("cost");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("IdWorker", "IdTask")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "IdTask" }, "fk_workers_has_tasks_tasks1_idx");

                    b.HasIndex(new[] { "IdWorker" }, "fk_workers_has_tasks_workers1_idx");

                    b.ToTable("workers_has_tasks", (string)null);
                });

            modelBuilder.Entity("ProjectersHasProject", b =>
                {
                    b.Property<int>("ProjectersIdUser")
                        .HasColumnType("int")
                        .HasColumnName("projecters_id_user");

                    b.Property<int>("ProjectsIdProject")
                        .HasColumnType("int")
                        .HasColumnName("projects_id_project");

                    b.HasKey("ProjectersIdUser", "ProjectsIdProject")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "ProjectersIdUser" }, "fk_projecters_has_projects_projecters1_idx");

                    b.HasIndex(new[] { "ProjectsIdProject" }, "fk_projecters_has_projects_projects1_idx");

                    b.ToTable("projecters_has_projects", (string)null);
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Admin", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.User", "UsersIdUserNavigation")
                        .WithOne("Admin")
                        .HasForeignKey("AutodorInfoSystem.Models.Admin", "UsersIdUser")
                        .IsRequired()
                        .HasConstraintName("fk_admins_users1");

                    b.Navigation("UsersIdUserNavigation");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.EquipmentHasTask", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.Equipment", "IdEquipmentNavigation")
                        .WithMany("EquipmentHasTasks")
                        .HasForeignKey("IdEquipment")
                        .IsRequired()
                        .HasConstraintName("fk_equipment_has_tasks_equipment1");

                    b.HasOne("AutodorInfoSystem.Models.Task", "IdTaskNavigation")
                        .WithMany("EquipmentHasTasks")
                        .HasForeignKey("IdTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_has_tasks_tasks1");

                    b.Navigation("IdEquipmentNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.MaterialsHasTask", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.Material", "IdMaterialNavigation")
                        .WithMany("MaterialsHasTasks")
                        .HasForeignKey("IdMaterial")
                        .IsRequired()
                        .HasConstraintName("fk_materials_has_tasks_materials1");

                    b.HasOne("AutodorInfoSystem.Models.Task", "IdTaskNavigation")
                        .WithMany("MaterialsHasTasks")
                        .HasForeignKey("IdTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_materials_has_tasks_tasks1");

                    b.Navigation("IdMaterialNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Projecter", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.User", "IdUserNavigation")
                        .WithOne("Projecter")
                        .HasForeignKey("AutodorInfoSystem.Models.Projecter", "IdUser")
                        .IsRequired()
                        .HasConstraintName("fk_projecters_users");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Task", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.Project", "IdProjectNavigation")
                        .WithMany("Tasks")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("fk_tasks_projects1");

                    b.Navigation("IdProjectNavigation");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.WorkersHasTask", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.Task", "IdTaskNavigation")
                        .WithMany("WorkersHasTasks")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("fk_workers_has_tasks_tasks1");

                    b.HasOne("AutodorInfoSystem.Models.Worker", "IdWorkerNavigation")
                        .WithMany("WorkersHasTasks")
                        .HasForeignKey("IdWorker")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workers_has_tasks_workers1");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdWorkerNavigation");
                });

            modelBuilder.Entity("ProjectersHasProject", b =>
                {
                    b.HasOne("AutodorInfoSystem.Models.Projecter", null)
                        .WithMany()
                        .HasForeignKey("ProjectersIdUser")
                        .IsRequired()
                        .HasConstraintName("fk_projecters_has_projects_projecters1");

                    b.HasOne("AutodorInfoSystem.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsIdProject")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_projecters_has_projects_projects1");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Equipment", b =>
                {
                    b.Navigation("EquipmentHasTasks");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Material", b =>
                {
                    b.Navigation("MaterialsHasTasks");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Project", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Task", b =>
                {
                    b.Navigation("EquipmentHasTasks");

                    b.Navigation("MaterialsHasTasks");

                    b.Navigation("WorkersHasTasks");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Projecter");
                });

            modelBuilder.Entity("AutodorInfoSystem.Models.Worker", b =>
                {
                    b.Navigation("WorkersHasTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
