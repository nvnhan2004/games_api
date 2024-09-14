using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class UpdateTblCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_crawl",
                table: "cn_categories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_error_game",
                table: "cn_categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_game",
                table: "cn_categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "url_crawl",
                table: "cn_categories",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_crawl",
                table: "cn_categories");

            migrationBuilder.DropColumn(
                name: "total_error_game",
                table: "cn_categories");

            migrationBuilder.DropColumn(
                name: "total_game",
                table: "cn_categories");

            migrationBuilder.DropColumn(
                name: "url_crawl",
                table: "cn_categories");
        }
    }
}
