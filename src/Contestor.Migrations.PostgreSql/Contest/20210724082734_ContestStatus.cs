using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class ContestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "contests",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "contests");
        }
    }
}
