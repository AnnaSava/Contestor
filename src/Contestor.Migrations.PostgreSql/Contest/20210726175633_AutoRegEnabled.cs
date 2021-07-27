using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class AutoRegEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "auto_reg_enabled",
                table: "contests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "auto_reg_enabled",
                table: "contests");
        }
    }
}
