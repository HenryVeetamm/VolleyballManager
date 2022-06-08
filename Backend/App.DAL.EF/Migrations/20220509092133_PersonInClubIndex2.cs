using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class PersonInClubIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonInClubs_ClubId_AppUserId",
                table: "PersonInClubs");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInClubs_ClubId_AppUserId",
                table: "PersonInClubs",
                columns: new[] { "ClubId", "AppUserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonInClubs_ClubId_AppUserId",
                table: "PersonInClubs");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInClubs_ClubId_AppUserId",
                table: "PersonInClubs",
                columns: new[] { "ClubId", "AppUserId" });
        }
    }
}
