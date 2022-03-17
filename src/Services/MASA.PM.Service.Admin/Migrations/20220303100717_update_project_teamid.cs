using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_project_teamid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId_ProjectId_IsDeleted",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                comment: "TeamId",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId_IsDeleted1",
                table: "EnvironmentClusterProjectApps",
                columns: new[] { "EnvironmentClusterProjectId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnvironmentClusterId_IsDeleted1",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.AlterColumn<byte>(
                name: "TeamId",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                comment: "TeamId",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "TeamId");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "EnvironmentClusterProjectApps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "System Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentClusterId_ProjectId_IsDeleted",
                table: "EnvironmentClusterProjectApps",
                columns: new[] { "EnvironmentClusterProjectId", "ProjectId", "IsDeleted" });
        }
    }
}
