using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class WorkWinnerFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nomination",
                table: "works",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "place",
                table: "works",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nomination",
                table: "works");

            migrationBuilder.DropColumn(
                name: "place",
                table: "works");
        }
    }
}
