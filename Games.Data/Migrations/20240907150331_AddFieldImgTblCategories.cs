using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class AddFieldImgTblCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "cn_categories",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img",
                table: "cn_categories");
        }
    }
}
