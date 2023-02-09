using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskListApi.Migrations
{
    /// <inheritdoc />
    public partial class TaskListEntityModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "DeadLineDays",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeadLineDays",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
