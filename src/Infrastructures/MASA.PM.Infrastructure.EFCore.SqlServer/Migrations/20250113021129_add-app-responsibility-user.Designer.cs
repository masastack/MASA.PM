﻿// <auto-generated />
using System;
using MASA.PM.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    [DbContext(typeof(PmDbContext))]
    [Migration("20250113021129_add-app-responsibility-user")]
    partial class addappresponsibilityuser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Description");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("Identity");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name");

                    b.Property<byte>("ServiceType")
                        .HasColumnType("tinyint")
                        .HasComment("ServiceType");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint")
                        .HasComment("Type");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.AppResponsibilityUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("AppResponsibilityUsers");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Cluster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Name");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Environment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Color");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentCluster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClusterId")
                        .HasColumnType("int")
                        .HasComment("Cluster Id");

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int")
                        .HasComment("Environment Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ClusterId" }, "IX_ClusterId");

                    b.HasIndex(new[] { "EnvironmentId" }, "IX_EnvironmentId");

                    b.ToTable("EnvironmentClusters");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentClusterProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EnvironmentClusterId")
                        .HasColumnType("int")
                        .HasComment("Environment cluster Id");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasComment("System Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EnvironmentClusterId" }, "IX_EnvironmentClusterId");

                    b.ToTable("EnvironmentClusterProjects");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentClusterProjectApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AppId")
                        .HasColumnType("int")
                        .HasComment("App Id");

                    b.Property<string>("AppSwaggerURL")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("Swagger URL");

                    b.Property<string>("AppURL")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("App URL");

                    b.Property<int>("EnvironmentClusterProjectId")
                        .HasColumnType("int")
                        .HasComment("Environment cluster project Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EnvironmentClusterProjectId" }, "IX_EnvironmentClusterId")
                        .HasDatabaseName("IX_EnvironmentClusterId1");

                    b.ToTable("EnvironmentClusterProjectApps");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentProjectTeam", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EnvironmentName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjectId", "TeamId", "EnvironmentName");

                    b.ToTable("EnvironmentProjectTeam");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Description");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Identity");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LabelCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("LabelCode");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.AppResponsibilityUser", b =>
                {
                    b.HasOne("MASA.PM.Infrastructure.Domain.Shared.Entities.App", "App")
                        .WithMany("ResponsibilityUsers")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.App", b =>
                {
                    b.Navigation("ResponsibilityUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
