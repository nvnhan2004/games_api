using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class AddFieldSEO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "cn_games",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_menu",
                table: "cn_games",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "cn_games",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "cn_categories",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_menu",
                table: "cn_categories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "cn_categories",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "cn_games");

            migrationBuilder.DropColumn(
                name: "is_menu",
                table: "cn_games");

            migrationBuilder.DropColumn(
                name: "title",
                table: "cn_games");

            migrationBuilder.DropColumn(
                name: "description",
                table: "cn_categories");

            migrationBuilder.DropColumn(
                name: "is_menu",
                table: "cn_categories");

            migrationBuilder.DropColumn(
                name: "title",
                table: "cn_categories");
        }
    }
}
