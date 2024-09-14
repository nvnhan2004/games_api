using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class AddFieldSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "cn_games",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "cn_categories",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slug",
                table: "cn_games");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "cn_categories");
        }
    }
}
