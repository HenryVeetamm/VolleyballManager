using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class TeamsFixed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Teams",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppUserId",
                table: "Teams",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_AppUserId",
                table: "Teams",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_AppUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_AppUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Teams");
        }
    }
}
