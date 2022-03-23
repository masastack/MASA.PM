using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_label_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Description");

            migrationBuilder.AddColumn<string>(
                name: "TypeCode",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "TypeCode");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "TypeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "Labels");
        }
    }
}
