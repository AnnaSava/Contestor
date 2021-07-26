using Microsoft.EntityFrameworkCore.Migrations;

namespace Contestor.Migrations.PostgreSql.Contest
{
    public partial class WorkFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_works_participants_participant_user_id_participant_contest_id",
                table: "works");

            migrationBuilder.DropIndex(
                name: "ix_works_participant_user_id_participant_contest_id",
                table: "works");

            migrationBuilder.DropColumn(
                name: "participant_contest_id",
                table: "works");

            migrationBuilder.DropColumn(
                name: "participant_user_id",
                table: "works");

            migrationBuilder.CreateIndex(
                name: "ix_works_participant_id_contest_id",
                table: "works",
                columns: new[] { "participant_id", "contest_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_works_participants_participant_user_id_participant_contest_id",
                table: "works",
                columns: new[] { "participant_id", "contest_id" },
                principalTable: "participants",
                principalColumns: new[] { "user_id", "contest_id" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_works_participants_participant_user_id_participant_contest_id",
                table: "works");

            migrationBuilder.DropIndex(
                name: "ix_works_participant_id_contest_id",
                table: "works");

            migrationBuilder.AddColumn<long>(
                name: "participant_contest_id",
                table: "works",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "participant_user_id",
                table: "works",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_works_participant_user_id_participant_contest_id",
                table: "works",
                columns: new[] { "participant_user_id", "participant_contest_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_works_participants_participant_user_id_participant_contest_id",
                table: "works",
                columns: new[] { "participant_user_id", "participant_contest_id" },
                principalTable: "participants",
                principalColumns: new[] { "user_id", "contest_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
