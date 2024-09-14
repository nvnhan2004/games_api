using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class AddTblCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cn_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    su_dung = table.Column<bool>(type: "bit", nullable: true),
                    so_thu_tu = table.Column<int>(type: "int", nullable: true),
                    stt_order = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    muc_luc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    categories_cap_tren_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cn_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_cn_categories_AspNetUsers_nguoi_chinh_sua_id",
                        column: x => x.nguoi_chinh_sua_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cn_categories_AspNetUsers_nguoi_tao_id",
                        column: x => x.nguoi_tao_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cn_categories_cn_categories_categories_cap_tren_id",
                        column: x => x.categories_cap_tren_id,
                        principalTable: "cn_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cn_categories_categories_cap_tren_id",
                table: "cn_categories",
                column: "categories_cap_tren_id");

            migrationBuilder.CreateIndex(
                name: "IX_cn_categories_nguoi_chinh_sua_id",
                table: "cn_categories",
                column: "nguoi_chinh_sua_id");

            migrationBuilder.CreateIndex(
                name: "IX_cn_categories_nguoi_tao_id",
                table: "cn_categories",
                column: "nguoi_tao_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cn_categories");
        }
    }
}
