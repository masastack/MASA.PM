using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Infrastructure.EFCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ReInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Identity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Identity"),
                    Type = table.Column<byte>(type: "tinyint", nullable: false, comment: "Type"),
                    ServiceType = table.Column<byte>(type: "tinyint", nullable: false, comment: "ServiceType"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Name"),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentClusterProjectApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentClusterProjectId = table.Column<int>(type: "int", nullable: false, comment: "Environment cluster project Id"),
                    AppId = table.Column<int>(type: "int", nullable: false, comment: "App Id"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentClusterId = table.Column<int>(type: "int", nullable: false, comment: "Environment cluster Id"),
                    ProjectId = table.Column<int>(type: "int", nullable: false, comment: "System Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusterProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentClusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentId = table.Column<int>(type: "int", nullable: false, comment: "Environment Id"),
                    ClusterId = table.Column<int>(type: "int", nullable: false, comment: "Cluster Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentClusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentProjectTeam",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    EnvironmentName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentProjectTeam", x => new { x.ProjectId, x.TeamId, x.EnvironmentName });
                });

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Color = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Color"),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Identity"),
                    LabelCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "LabelCode"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Description"),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppResponsibilityUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppResponsibilityUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppResponsibilityUsers_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppResponsibilityUsers_AppId",
                table: "AppResponsibilityUsers",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppResponsibilityUsers");

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

            migrationBuilder.DropTable(
                name: "Apps");
        }
    }
}
