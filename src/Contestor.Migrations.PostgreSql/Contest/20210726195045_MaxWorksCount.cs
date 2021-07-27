using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class MaxWorksCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "max_works_count",
                table: "contests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_works_count",
                table: "contests");
        }
    }
}
