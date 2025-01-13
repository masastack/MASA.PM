using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class addappresponsibilityuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationEventLog");

            migrationBuilder.AlterColumn<string>(
                name: "AppURL",
                table: "EnvironmentClusterProjectApps",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "App URL",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "App URL");

            migrationBuilder.AlterColumn<string>(
                name: "AppSwaggerURL",
                table: "EnvironmentClusterProjectApps",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Swagger URL",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Swagger URL");

            migrationBuilder.AlterColumn<string>(
                name: "Identity",
                table: "Apps",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Identity",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Identity");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppResponsibilityUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AppURL",
                table: "EnvironmentClusterProjectApps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "App URL",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "App URL");

            migrationBuilder.AlterColumn<string>(
                name: "AppSwaggerURL",
                table: "EnvironmentClusterProjectApps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Swagger URL",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Swagger URL");

            migrationBuilder.AlterColumn<string>(
                name: "Identity",
                table: "Apps",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Identity",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "Identity");

            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpandContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimesSent = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventId_Version",
                table: "IntegrationEventLog",
                columns: new[] { "EventId", "RowVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_State_MTime",
                table: "IntegrationEventLog",
                columns: new[] { "State", "ModificationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_State_TimesSent_MTime",
                table: "IntegrationEventLog",
                columns: new[] { "State", "TimesSent", "ModificationTime" });
        }
    }
}
