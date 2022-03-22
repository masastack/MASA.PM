using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASA.PM.Service.Admin.Migrations
{
    public partial class add_label_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "LabelId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "LabelId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "EventLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "EventLogs",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name"),
                    TypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "TypeCode"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "TypeName"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    Modifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Name",
                table: "ProjectTypes",
                columns: new[] { "Name", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TypeCode",
                table: "ProjectTypes",
                columns: new[] { "TypeCode", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTypes");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "EventLogs");

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
