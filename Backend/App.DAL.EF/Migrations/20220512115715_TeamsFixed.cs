using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class TeamsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedComparisons_ComparerId",
                table: "SavedComparisons");

            migrationBuilder.DropIndex(
                name: "IX_PersonInWorkouts_WorkOutId",
                table: "PersonInWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_PersonInTeams_TeamId",
                table: "PersonInTeams");

            migrationBuilder.DropIndex(
                name: "IX_PersonInMatches_MatchId",
                table: "PersonInMatches");

            migrationBuilder.AddColumn<bool>(
                name: "OwnTeam",
                table: "Teams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SavedComparisons_ComparerId_ComparableId",
                table: "SavedComparisons",
                columns: new[] { "ComparerId", "ComparableId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonInWorkouts_WorkOutId_AppUserId",
                table: "PersonInWorkouts",
                columns: new[] { "WorkOutId", "AppUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonInTeams_TeamId_AppUserId",
                table: "PersonInTeams",
                columns: new[] { "TeamId", "AppUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonInMatches_MatchId_AppUserId",
                table: "PersonInMatches",
                columns: new[] { "MatchId", "AppUserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedComparisons_ComparerId_ComparableId",
                table: "SavedComparisons");

            migrationBuilder.DropIndex(
                name: "IX_PersonInWorkouts_WorkOutId_AppUserId",
                table: "PersonInWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_PersonInTeams_TeamId_AppUserId",
                table: "PersonInTeams");

            migrationBuilder.DropIndex(
                name: "IX_PersonInMatches_MatchId_AppUserId",
                table: "PersonInMatches");

            migrationBuilder.DropColumn(
                name: "OwnTeam",
                table: "Teams");

            migrationBuilder.CreateIndex(
                name: "IX_SavedComparisons_ComparerId",
                table: "SavedComparisons",
                column: "ComparerId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInWorkouts_WorkOutId",
                table: "PersonInWorkouts",
                column: "WorkOutId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInTeams_TeamId",
                table: "PersonInTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInMatches_MatchId",
                table: "PersonInMatches",
                column: "MatchId");
        }
    }
}
