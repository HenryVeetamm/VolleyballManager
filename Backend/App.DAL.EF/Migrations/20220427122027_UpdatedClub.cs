using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class UpdatedClub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Clubs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_AppUserId",
                table: "Clubs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_AspNetUsers_AppUserId",
                table: "Clubs",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_AspNetUsers_AppUserId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_AppUserId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Clubs");
        }
    }
}
