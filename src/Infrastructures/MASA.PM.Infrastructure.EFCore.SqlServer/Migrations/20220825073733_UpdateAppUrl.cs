using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class UpdateAppUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SwaggerUrl",
                table: "Apps");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Apps");

            migrationBuilder.AddColumn<string>(
                name: "AppSwaggerURL",
                table: "EnvironmentClusterProjectApps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Swagger URL");

            migrationBuilder.AddColumn<string>(
                name: "AppURL",
                table: "EnvironmentClusterProjectApps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "App URL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppSwaggerURL",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.DropColumn(
                name: "AppURL",
                table: "EnvironmentClusterProjectApps");

            migrationBuilder.AddColumn<string>(
                name: "SwaggerUrl",
                table: "Apps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "SwaggerUrl");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Apps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Url");
        }
    }
}
