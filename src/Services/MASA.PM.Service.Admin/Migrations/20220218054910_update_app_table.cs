using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_app_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectApps");

            migrationBuilder.CreateTable(
                name: "EnvironmentClusterProjectApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentClusterProjectId = table.Column<int>(type: "int", nullable: false, comment: "Environment cluster project Id"),
                    ProjectId = table.Column<int>(type: "int", nullable: false, comment: "System Id"),
                    AppId = table.Column<int>(type: "int", nullable: false, comment: "App Id"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusterProjectApps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId_ProjectId_IsDeleted",
                table: "EnvironmentClusterProjectApps",
                columns: new[] { "EnvironmentClusterProjectId", "ProjectId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvironmentClusterProjectApps");

            migrationBuilder.CreateTable(
                name: "ProjectApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false, comment: "App Id"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false, comment: "Project Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectApps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemId_IsDeleted",
                table: "ProjectApps",
                columns: new[] { "ProjectId", "IsDeleted" });
        }
    }
}
