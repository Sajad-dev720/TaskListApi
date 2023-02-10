using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskListApi.Migrations
{
    /// <inheritdoc />
    public partial class Typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descrption",
                table: "Tasks",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tasks",
                newName: "Descrption");
        }
    }
}
