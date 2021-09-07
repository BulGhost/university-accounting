using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityAccounting.DAL.Migrations
{
    public partial class StudentsQuantityFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FUNCTION dbo.FindStudentsQuantityInGroup (@GroupId INT) " +
                                 "RETURNS INT AS " +
                                 "BEGIN " +
                                    "DECLARE @StudentsQuantity INT; " +
                                    "SELECT @StudentsQuantity = COUNT(*) " +
                                    "FROM Students " +
                                    "WHERE GroupId = @GroupId " +
                                    "RETURN @StudentsQuantity " +
                                 "END");

            migrationBuilder.AddColumn<int>(
                name: "StudentsQuantity",
                table: "Groups",
                type: "int",
                nullable: false,
                computedColumnSql: "dbo.FindStudentsQuantityInGroup([Id])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentsQuantity",
                table: "Groups");

            migrationBuilder.Sql("DROP FUNCTION dbo.FindStudentsQuantityInGroup");
        }
    }
}
