using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_project_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Projects");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "Type");
        }
    }
}
