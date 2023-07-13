using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class AddEnvironmentProjectTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Projects");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvironmentProjectTeam");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "TeamId");
        }
    }
}
