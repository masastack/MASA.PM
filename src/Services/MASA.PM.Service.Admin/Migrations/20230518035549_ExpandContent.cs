using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class ExpandContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "index_state_timessent_modificationtime",
                table: "IntegrationEventLog",
                newName: "IX_State_TimesSent_MTime");

            migrationBuilder.RenameIndex(
                name: "index_state_modificationtime",
                table: "IntegrationEventLog",
                newName: "IX_State_MTime");

            migrationBuilder.RenameIndex(
                name: "index_eventid_version",
                table: "IntegrationEventLog",
                newName: "IX_EventId_Version");

            migrationBuilder.AddColumn<string>(
                name: "ExpandContent",
                table: "IntegrationEventLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpandContent",
                table: "IntegrationEventLog");

            migrationBuilder.RenameIndex(
                name: "IX_State_TimesSent_MTime",
                table: "IntegrationEventLog",
                newName: "index_state_timessent_modificationtime");

            migrationBuilder.RenameIndex(
                name: "IX_State_MTime",
                table: "IntegrationEventLog",
                newName: "index_state_modificationtime");

            migrationBuilder.RenameIndex(
                name: "IX_EventId_Version",
                table: "IntegrationEventLog",
                newName: "index_eventid_version");
        }
    }
}
