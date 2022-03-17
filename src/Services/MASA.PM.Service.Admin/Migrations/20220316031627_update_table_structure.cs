using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_table_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "TeamProjects");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentId_IsDeleted",
                table: "EnvironmentClusters");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId_IsDeleted",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId_IsDeleted1",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "EnvironmentClusters");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldComment: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Identity");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Environments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldComment: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Clusters",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldComment: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Apps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Url",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "Url");

            migrationBuilder.AlterColumn<string>(
                name: "SwaggerUrl",
                table: "Apps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "SwaggerUrl",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "SwaggerUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Apps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldComment: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_ClusterId",
                table: "EnvironmentClusters",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentId",
                table: "EnvironmentClusters",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId",
                table: "EnvironmentClusterProjects",
                column: "EnvironmentClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId1",
                table: "EnvironmentClusterProjectApps",
                column: "EnvironmentClusterProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClusterId",
                table: "EnvironmentClusters");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentId",
                table: "EnvironmentClusters");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId",
                table: "EnvironmentClusterProjects");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId1",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "Identity",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Environments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EnvironmentClusters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "EnvironmentClusters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "EnvironmentClusters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is default");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnvironmentClusters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "EnvironmentClusters",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<Guid>(
                name: "Modifier",
                table: "EnvironmentClusters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EnvironmentClusterProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "EnvironmentClusterProjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnvironmentClusterProjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "EnvironmentClusterProjects",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<Guid>(
                name: "Modifier",
                table: "EnvironmentClusterProjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EnvironmentClusterProjectApps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "EnvironmentClusterProjectApps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnvironmentClusterProjectApps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "EnvironmentClusterProjectApps",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<Guid>(
                name: "Modifier",
                table: "EnvironmentClusterProjectApps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Clusters",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                comment: "Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Apps",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "Url",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Url");

            migrationBuilder.AlterColumn<string>(
                name: "SwaggerUrl",
                table: "Apps",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "SwaggerUrl",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "SwaggerUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Apps",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                comment: "Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Description");

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false, comment: "Team Id"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Id"),
                    IsAdministrator = table.Column<bool>(type: "bit", nullable: false, comment: "Is administrator")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "TeamProjects",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false, comment: "Team Id"),
                    ProjectId = table.Column<int>(type: "int", nullable: false, comment: "Project Id"),
                    AuthorizationTeamId = table.Column<int>(type: "int", nullable: false, comment: "Authorization team Id"),
                    AuthorizationAccept = table.Column<bool>(type: "bit", nullable: false, comment: "Authorization accept"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProjects", x => new { x.TeamId, x.ProjectId, x.AuthorizationTeamId });
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "AvatarPath"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "Description"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentId_IsDeleted",
                table: "EnvironmentClusters",
                columns: new[] { "EnvironmentId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId_IsDeleted",
                table: "EnvironmentClusterProjects",
                columns: new[] { "EnvironmentClusterId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId_IsDeleted1",
                table: "EnvironmentClusterProjectApps",
                columns: new[] { "EnvironmentClusterProjectId", "IsDeleted" });
        }
    }
}
