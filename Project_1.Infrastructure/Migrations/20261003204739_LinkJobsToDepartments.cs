using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_1.Migrations
{
    /// <inheritdoc />
    public partial class LinkJobsToDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DepartmentId",
                table: "Jobs",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Departments_DepartmentId",
                table: "Jobs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Departments_DepartmentId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_DepartmentId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Jobs");
        }
    }
}
