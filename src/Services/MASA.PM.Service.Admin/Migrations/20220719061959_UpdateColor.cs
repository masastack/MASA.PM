using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class UpdateColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LabelCode",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                comment: "LabelCode",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "LabelId");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Environments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Color",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldComment: "Color");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LabelCode",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                comment: "LabelId",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "LabelCode");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Environments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "Color",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Color");
        }
    }
}
