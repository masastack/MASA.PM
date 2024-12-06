﻿// <auto-generated />
using System;
using MASA.PM.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.Migrations
{
    [DbContext(typeof(PmDbContext))]
    [Migration("20241206081216_pm-init")]
    partial class pminit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("Description");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("Identity");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("Name");

                    b.Property<short>("ServiceType")
                        .HasColumnType("smallint")
                        .HasComment("ServiceType");

                    b.Property<short>("Type")
                        .HasColumnType("smallint")
                        .HasComment("Type");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Cluster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("Name");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Environment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasComment("Color");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasComment("Description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentCluster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClusterId")
                        .HasColumnType("integer")
                        .HasComment("Cluster Id");

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("integer")
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
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EnvironmentClusterId")
                        .HasColumnType("integer")
                        .HasComment("Environment cluster Id");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer")
                        .HasComment("System Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EnvironmentClusterId" }, "IX_EnvironmentClusterId");

                    b.ToTable("EnvironmentClusterProjects");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentClusterProjectApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AppId")
                        .HasColumnType("integer")
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
                        .HasColumnType("integer")
                        .HasComment("Environment cluster project Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EnvironmentClusterProjectId" }, "IX_EnvironmentClusterId")
                        .HasDatabaseName("IX_EnvironmentClusterId1");

                    b.ToTable("EnvironmentClusterProjectApps");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.EnvironmentProjectTeam", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<string>("EnvironmentName")
                        .HasColumnType("text");

                    b.HasKey("ProjectId", "TeamId", "EnvironmentName");

                    b.ToTable("EnvironmentProjectTeam");
                });

            modelBuilder.Entity("MASA.PM.Infrastructure.Domain.Shared.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Creator")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasComment("Description");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasComment("Identity");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LabelCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasComment("LabelCode");

                    b.Property<DateTime>("ModificationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasComment("Name");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}
