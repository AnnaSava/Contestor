using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class WorksCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "works_count",
                table: "participants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "works_count",
                table: "participants");
        }
    }
}
