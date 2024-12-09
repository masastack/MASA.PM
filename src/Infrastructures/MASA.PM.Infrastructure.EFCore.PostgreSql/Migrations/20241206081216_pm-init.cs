using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.Migrations
{
    public partial class pminit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Identity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Identity"),
                    Type = table.Column<short>(type: "smallint", nullable: false, comment: "Type"),
                    ServiceType = table.Column<short>(type: "smallint", nullable: false, comment: "ServiceType"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Creator = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modifier = table.Column<Guid>(type: "uuid", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Name"),
                    Creator = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modifier = table.Column<Guid>(type: "uuid", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentClusterProjectApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnvironmentClusterProjectId = table.Column<int>(type: "integer", nullable: false, comment: "Environment cluster project Id"),
                    AppId = table.Column<int>(type: "integer", nullable: false, comment: "App Id"),
                    AppURL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "App URL"),
                    AppSwaggerURL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Swagger URL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusterProjectApps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentClusterProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnvironmentClusterId = table.Column<int>(type: "integer", nullable: false, comment: "Environment cluster Id"),
                    ProjectId = table.Column<int>(type: "integer", nullable: false, comment: "System Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusterProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentClusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnvironmentId = table.Column<int>(type: "integer", nullable: false, comment: "Environment Id"),
                    ClusterId = table.Column<int>(type: "integer", nullable: false, comment: "Cluster Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentProjectTeam",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    EnvironmentName = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentProjectTeam", x => new { x.ProjectId, x.TeamId, x.EnvironmentName });
                });

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Color"),
                    Creator = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modifier = table.Column<Guid>(type: "uuid", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Identity"),
                    LabelCode = table.Column<string>(type: "text", nullable: false, comment: "LabelCode"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Creator = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modifier = table.Column<Guid>(type: "uuid", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId1",
                table: "EnvironmentClusterProjectApps",
                column: "EnvironmentClusterProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId",
                table: "EnvironmentClusterProjects",
                column: "EnvironmentClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClusterId",
                table: "EnvironmentClusters",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentId",
                table: "EnvironmentClusters",
                column: "EnvironmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "EnvironmentClusterProjectApps");

            migrationBuilder.DropTable(
                name: "EnvironmentClusterProjects");

            migrationBuilder.DropTable(
                name: "EnvironmentClusters");

            migrationBuilder.DropTable(
                name: "EnvironmentProjectTeam");

            migrationBuilder.DropTable(
                name: "Environments");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
