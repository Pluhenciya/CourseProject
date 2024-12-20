﻿using System;
using System.Collections.Generic;
using AutodorInfoApi.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AutodorInfoApi.Data;

public partial class AutodorContext : DbContext
{
    public AutodorContext()
    {
    }

    public AutodorContext(DbContextOptions<AutodorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<EquipmentHasTask> EquipmentHasTasks { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialsHasTask> MaterialsHasTasks { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Projecter> Projecters { get; set; }

    public virtual DbSet<Models.Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkersHasTask> WorkersHasTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.UsersIdUser).HasName("PRIMARY");

            entity.ToTable("admins");

            entity.Property(e => e.UsersIdUser)
                .ValueGeneratedNever()
                .HasColumnName("users_id_user");

            entity.HasOne(d => d.UsersIdUserNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UsersIdUser)
                .HasConstraintName("fk_admins_users1");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.IdEquipment).HasName("PRIMARY");

            entity.ToTable("equipment");

            entity.Property(e => e.IdEquipment).HasColumnName("id_equipment");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<EquipmentHasTask>(entity =>
        {
            entity.HasKey(e => new { e.IdEquipment, e.IdTask })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("equipment_has_tasks");

            entity.HasIndex(e => e.IdEquipment, "fk_equipment_has_tasks_equipment1_idx");

            entity.HasIndex(e => e.IdTask, "fk_equipment_has_tasks_tasks1_idx");

            entity.Property(e => e.IdEquipment).HasColumnName("id_equipment");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.Cost)
                .HasPrecision(11, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("'1'")
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdEquipmentNavigation).WithMany(p => p.EquipmentHasTasks)
                .HasForeignKey(d => d.IdEquipment)
                .HasConstraintName("fk_equipment_has_tasks_equipment1");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.EquipmentHasTasks)
                .HasForeignKey(d => d.IdTask)
                .HasConstraintName("fk_equipment_has_tasks_tasks1");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PRIMARY");

            entity.ToTable("materials");

            entity.Property(e => e.IdMaterial).HasColumnName("id_material");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.MeasurementUnit)
                .HasMaxLength(5)
                .HasColumnName("measurement_unit");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<MaterialsHasTask>(entity =>
        {
            entity.HasKey(e => new { e.IdMaterial, e.IdTask })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("materials_has_tasks");

            entity.HasIndex(e => e.IdMaterial, "fk_materials_has_tasks_materials1_idx");

            entity.HasIndex(e => e.IdTask, "fk_materials_has_tasks_tasks1_idx");

            entity.Property(e => e.IdMaterial).HasColumnName("id_material");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.Cost)
                .HasPrecision(11, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.MaterialsHasTasks)
                .HasForeignKey(d => d.IdMaterial)
                .HasConstraintName("fk_materials_has_tasks_materials1");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.MaterialsHasTasks)
                .HasForeignKey(d => d.IdTask)
                .HasConstraintName("fk_materials_has_tasks_tasks1");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("PRIMARY");

            entity.ToTable("projects");

            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Cost)
                .HasPrecision(11, 2)
                .HasColumnName("cost");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_completed");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Projecter>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("projecters");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasMaxLength(45)
                .HasColumnName("surname");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Projecter)
                .HasForeignKey<Projecter>(d => d.IdUser)
                .HasConstraintName("fk_projecters_users");

            entity.HasMany(d => d.ProjectsIdProjects).WithMany(p => p.ProjectersIdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectersHasProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectsIdProject")
                        .HasConstraintName("fk_projecters_has_projects_projects1"),
                    l => l.HasOne<Projecter>().WithMany()
                        .HasForeignKey("ProjectersIdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_projecters_has_projects_projecters1"),
                    j =>
                    {
                        j.HasKey("ProjectersIdUser", "ProjectsIdProject")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("projecters_has_projects");
                        j.HasIndex(new[] { "ProjectersIdUser" }, "fk_projecters_has_projects_projecters1_idx");
                        j.HasIndex(new[] { "ProjectsIdProject" }, "fk_projecters_has_projects_projects1_idx");
                        j.IndexerProperty<int>("ProjectersIdUser").HasColumnName("projecters_id_user");
                        j.IndexerProperty<int>("ProjectsIdProject").HasColumnName("projects_id_project");
                    });
        });

        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("PRIMARY");

            entity.ToTable("tasks");

            entity.HasIndex(e => e.IdProject, "fk_tasks_projects1_idx");

            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.Cost)
                .HasPrecision(11, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdProject)
                .HasConstraintName("fk_tasks_projects1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "login_UNIQUE").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.IdWorker).HasName("PRIMARY");

            entity.ToTable("workers");

            entity.Property(e => e.IdWorker).HasColumnName("id_worker");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Salary).HasColumnName("salary");
        });

        modelBuilder.Entity<WorkersHasTask>(entity =>
        {
            entity.HasKey(e => new { e.IdWorker, e.IdTask })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("workers_has_tasks");

            entity.HasIndex(e => e.IdTask, "fk_workers_has_tasks_tasks1_idx");

            entity.HasIndex(e => e.IdWorker, "fk_workers_has_tasks_workers1_idx");

            entity.Property(e => e.IdWorker).HasColumnName("id_worker");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.Cost)
                .HasPrecision(11, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.WorkersHasTasks)
                .HasForeignKey(d => d.IdTask)
                .HasConstraintName("fk_workers_has_tasks_tasks1");

            entity.HasOne(d => d.IdWorkerNavigation).WithMany(p => p.WorkersHasTasks)
                .HasForeignKey(d => d.IdWorker)
                .HasConstraintName("fk_workers_has_tasks_workers1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
