using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class update_label_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "TypeName",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "TypeName");

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "TypeCode",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "TypeCode");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Labels",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Labels");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Labels",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "TypeName",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "TypeName");

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "Labels",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "TypeCode",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "TypeCode");
        }
    }
}
