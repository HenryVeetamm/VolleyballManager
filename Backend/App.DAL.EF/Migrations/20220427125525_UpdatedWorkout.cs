using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.App.EF.Migrations
{
    public partial class UpdatedWorkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Workouts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_AppUserId",
                table: "Workouts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_AspNetUsers_AppUserId",
                table: "Workouts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_AspNetUsers_AppUserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_AppUserId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Workouts");
        }
    }
}
