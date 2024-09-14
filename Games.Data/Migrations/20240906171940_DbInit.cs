using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Games.Data.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    super_admin = table.Column<bool>(type: "bit", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qtht_dieu_huong",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duong_dan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    icon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    so_thu_tu = table.Column<int>(type: "int", nullable: true),
                    stt_order = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cap_dieu_huong = table.Column<int>(type: "int", nullable: false),
                    muc_luc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    dieu_huong_cap_tren_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    super_admin = table.Column<bool>(type: "bit", nullable: true),
                    is_router = table.Column<bool>(type: "bit", nullable: false),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qtht_dieu_huong", x => x.id);
                    table.ForeignKey(
                        name: "FK_qtht_dieu_huong_AspNetUsers_nguoi_chinh_sua_id",
                        column: x => x.nguoi_chinh_sua_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_qtht_dieu_huong_AspNetUsers_nguoi_tao_id",
                        column: x => x.nguoi_tao_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_qtht_dieu_huong_qtht_dieu_huong_dieu_huong_cap_tren_id",
                        column: x => x.dieu_huong_cap_tren_id,
                        principalTable: "qtht_dieu_huong",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "qtht_nhom_nguoi_dung",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nguoi_tao_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nguoi_chinh_sua_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngay_chinh_sua = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qtht_nhom_nguoi_dung", x => x.id);
                    table.ForeignKey(
                        name: "FK_qtht_nhom_nguoi_dung_AspNetUsers_nguoi_chinh_sua_id",
                        column: x => x.nguoi_chinh_sua_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_qtht_nhom_nguoi_dung_AspNetUsers_nguoi_tao_id",
                        column: x => x.nguoi_tao_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "qtht_nguoi_dung_2_nhom_nguoi_dung",
                columns: table => new
                {
                    nguoi_dung_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nhom_nguoi_dung_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qtht_nguoi_dung_2_nhom_nguoi_dung", x => new { x.nguoi_dung_id, x.nhom_nguoi_dung_id });
                    table.ForeignKey(
                        name: "FK_qtht_nguoi_dung_2_nhom_nguoi_dung_AspNetUsers_nguoi_dung_id",
                        column: x => x.nguoi_dung_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_qtht_nguoi_dung_2_nhom_nguoi_dung_qtht_nhom_nguoi_dung_nhom_nguoi_dung_id",
                        column: x => x.nhom_nguoi_dung_id,
                        principalTable: "qtht_nhom_nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qtht_nhom_nguoi_dung_2_dieu_huong",
                columns: table => new
                {
                    nhom_nguoi_dung_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dieu_huong_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qtht_nhom_nguoi_dung_2_dieu_huong", x => new { x.nhom_nguoi_dung_id, x.dieu_huong_id });
                    table.ForeignKey(
                        name: "FK_qtht_nhom_nguoi_dung_2_dieu_huong_qtht_dieu_huong_dieu_huong_id",
                        column: x => x.dieu_huong_id,
                        principalTable: "qtht_dieu_huong",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_qtht_nhom_nguoi_dung_2_dieu_huong_qtht_nhom_nguoi_dung_nhom_nguoi_dung_id",
                        column: x => x.nhom_nguoi_dung_id,
                        principalTable: "qtht_nhom_nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_dieu_huong_dieu_huong_cap_tren_id",
                table: "qtht_dieu_huong",
                column: "dieu_huong_cap_tren_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_dieu_huong_nguoi_chinh_sua_id",
                table: "qtht_dieu_huong",
                column: "nguoi_chinh_sua_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_dieu_huong_nguoi_tao_id",
                table: "qtht_dieu_huong",
                column: "nguoi_tao_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_nguoi_dung_2_nhom_nguoi_dung_nhom_nguoi_dung_id",
                table: "qtht_nguoi_dung_2_nhom_nguoi_dung",
                column: "nhom_nguoi_dung_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_nhom_nguoi_dung_nguoi_chinh_sua_id",
                table: "qtht_nhom_nguoi_dung",
                column: "nguoi_chinh_sua_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_nhom_nguoi_dung_nguoi_tao_id",
                table: "qtht_nhom_nguoi_dung",
                column: "nguoi_tao_id");

            migrationBuilder.CreateIndex(
                name: "IX_qtht_nhom_nguoi_dung_2_dieu_huong_dieu_huong_id",
                table: "qtht_nhom_nguoi_dung_2_dieu_huong",
                column: "dieu_huong_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "qtht_nguoi_dung_2_nhom_nguoi_dung");

            migrationBuilder.DropTable(
                name: "qtht_nhom_nguoi_dung_2_dieu_huong");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "qtht_dieu_huong");

            migrationBuilder.DropTable(
                name: "qtht_nhom_nguoi_dung");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
