using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineWatches.Migrations
{
    public partial class Secondary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Watches");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Watches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Watches");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserId",
                table: "Watches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
