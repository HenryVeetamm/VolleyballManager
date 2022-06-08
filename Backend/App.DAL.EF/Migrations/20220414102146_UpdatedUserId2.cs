using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class UpdatedUserId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AppUserId",
                table: "Matches",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_AppUserId",
                table: "Matches",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_AppUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AppUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Matches");
        }
    }
}
