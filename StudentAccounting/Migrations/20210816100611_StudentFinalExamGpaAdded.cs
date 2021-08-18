using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAccounting.Migrations
{
    public partial class StudentFinalExamGpaAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FinalExamGpa",
                table: "Students",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalExamGpa",
                table: "Students");
        }
    }
}
