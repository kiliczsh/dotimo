using Microsoft.EntityFrameworkCore.Migrations;

namespace dotimo.Data.Migrations
{
    public partial class AddMissingForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CheckUps_WatchId",
                table: "CheckUps",
                column: "WatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckUps_Watches_WatchId",
                table: "CheckUps",
                column: "WatchId",
                principalTable: "Watches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckUps_Watches_WatchId",
                table: "CheckUps");

            migrationBuilder.DropIndex(
                name: "IX_CheckUps_WatchId",
                table: "CheckUps");
        }
    }
}
