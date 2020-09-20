using Microsoft.EntityFrameworkCore.Migrations;

namespace dotimo.Data.Migrations
{
    public partial class RemoveEnumIdFromWatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonitoringTimePeriodId",
                table: "Watches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonitoringTimePeriodId",
                table: "Watches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
