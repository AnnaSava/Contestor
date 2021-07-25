using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class ContestFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "works",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "round_number",
                table: "works",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "round_number",
                table: "contests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "works");

            migrationBuilder.DropColumn(
                name: "round_number",
                table: "works");

            migrationBuilder.DropColumn(
                name: "round_number",
                table: "contests");
        }
    }
}
