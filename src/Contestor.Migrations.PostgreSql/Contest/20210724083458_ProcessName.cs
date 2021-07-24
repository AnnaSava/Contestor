using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class ProcessName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "process_name",
                table: "contests",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "process_name",
                table: "contests");
        }
    }
}
