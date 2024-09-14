using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class AddTblGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cn_games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    iframe = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    su_dung = table.Column<bool>(type: "bit", nullable: true),
                    so_thu_tu = table.Column<int>(type: "int", nullable: true),
                    so_lan_choi = table.Column<int>(type: "int", nullable: true),
                    like = table.Column<int>(type: "int", nullable: true),
                    dislike = table.Column<int>(type: "int", nullable: true),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_trending = table.Column<bool>(type: "bit", nullable: true),
                    is_new = table.Column<bool>(type: "bit", nullable: true),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cn_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_cn_games_AspNetUsers_nguoi_chinh_sua_id",
                        column: x => x.nguoi_chinh_sua_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cn_games_AspNetUsers_nguoi_tao_id",
                        column: x => x.nguoi_tao_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cn_games_cn_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "cn_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cn_games_category_id",
                table: "cn_games",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_cn_games_nguoi_chinh_sua_id",
                table: "cn_games",
                column: "nguoi_chinh_sua_id");

            migrationBuilder.CreateIndex(
                name: "IX_cn_games_nguoi_tao_id",
                table: "cn_games",
                column: "nguoi_tao_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cn_games");
        }
    }
}
