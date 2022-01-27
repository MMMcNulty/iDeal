using Microsoft.EntityFrameworkCore.Migrations;

namespace iDeal.Migrations
{
    public partial class iDealDBV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameOutcome",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "PlayerWin",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerWin",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "GameOutcome",
                table: "Games",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
