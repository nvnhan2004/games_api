﻿// <auto-generated />
using System;
using Games.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Games.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240907164615_AddTblGames")]
    partial class AddTblGames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Games.Data.Models.ChucNang.Categories", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("cap")
                        .HasColumnType("int");

                    b.Property<Guid?>("categories_cap_tren_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("img")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("mo_ta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("muc_luc")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("so_thu_tu")
                        .HasColumnType("int");

                    b.Property<string>("stt_order")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("su_dung")
                        .HasColumnType("bit");

                    b.Property<string>("ten")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("id");

                    b.HasIndex("categories_cap_tren_id");

                    b.HasIndex("nguoi_chinh_sua_id");

                    b.HasIndex("nguoi_tao_id");

                    b.ToTable("cn_categories");
                });

            modelBuilder.Entity("Games.Data.Models.ChucNang.Games", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("category_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("dislike")
                        .HasColumnType("int");

                    b.Property<string>("iframe")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("is_new")
                        .HasColumnType("bit");

                    b.Property<bool?>("is_trending")
                        .HasColumnType("bit");

                    b.Property<int?>("like")
                        .HasColumnType("int");

                    b.Property<string>("mo_ta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("so_lan_choi")
                        .HasColumnType("int");

                    b.Property<int?>("so_thu_tu")
                        .HasColumnType("int");

                    b.Property<bool?>("su_dung")
                        .HasColumnType("bit");

                    b.Property<string>("ten")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("nguoi_chinh_sua_id");

                    b.HasIndex("nguoi_tao_id");

                    b.ToTable("cn_games");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.DieuHuong", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("cap_dieu_huong")
                        .HasColumnType("int");

                    b.Property<Guid?>("dieu_huong_cap_tren_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("duong_dan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("icon")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("is_router")
                        .HasColumnType("bit");

                    b.Property<string>("ma")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("mo_ta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("muc_luc")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("so_thu_tu")
                        .HasColumnType("int");

                    b.Property<string>("stt_order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("super_admin")
                        .HasColumnType("bit");

                    b.Property<string>("ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("dieu_huong_cap_tren_id");

                    b.HasIndex("nguoi_chinh_sua_id");

                    b.HasIndex("nguoi_tao_id");

                    b.ToTable("qtht_dieu_huong");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NguoiDung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phonenumber");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("username");

                    b.Property<string>("full_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("super_admin")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NguoiDung_2_NhomNguoiDung", b =>
                {
                    b.Property<Guid>("nguoi_dung_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("nhom_nguoi_dung_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("nguoi_dung_id", "nhom_nguoi_dung_id");

                    b.HasIndex("nhom_nguoi_dung_id");

                    b.ToTable("qtht_nguoi_dung_2_nhom_nguoi_dung");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NhomNguoiDung", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ma")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("mota")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("nguoi_chinh_sua_id");

                    b.HasIndex("nguoi_tao_id");

                    b.ToTable("qtht_nhom_nguoi_dung");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NhomNguoiDung_2_DieuHuong", b =>
                {
                    b.Property<Guid>("nhom_nguoi_dung_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("dieu_huong_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("nhom_nguoi_dung_id", "dieu_huong_id");

                    b.HasIndex("dieu_huong_id");

                    b.ToTable("qtht_nhom_nguoi_dung_2_dieu_huong");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.Roles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ngay_chinh_sua")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ngay_tao")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("nguoi_chinh_sua_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("nguoi_tao_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Games.Data.Models.ChucNang.Categories", b =>
                {
                    b.HasOne("Games.Data.Models.ChucNang.Categories", "categories_cap_tren")
                        .WithMany("ds_categories_cap_duoi")
                        .HasForeignKey("categories_cap_tren_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_chinh_sua")
                        .WithMany()
                        .HasForeignKey("nguoi_chinh_sua_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_tao")
                        .WithMany()
                        .HasForeignKey("nguoi_tao_id");

                    b.Navigation("categories_cap_tren");

                    b.Navigation("nguoi_chinh_sua");

                    b.Navigation("nguoi_tao");
                });

            modelBuilder.Entity("Games.Data.Models.ChucNang.Games", b =>
                {
                    b.HasOne("Games.Data.Models.ChucNang.Categories", "categories")
                        .WithMany("ds_games")
                        .HasForeignKey("category_id")
                        .IsRequired();

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_chinh_sua")
                        .WithMany()
                        .HasForeignKey("nguoi_chinh_sua_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_tao")
                        .WithMany()
                        .HasForeignKey("nguoi_tao_id");

                    b.Navigation("categories");

                    b.Navigation("nguoi_chinh_sua");

                    b.Navigation("nguoi_tao");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.DieuHuong", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.DieuHuong", "dieu_huong_cap_tren")
                        .WithMany("ds_dieu_huong_cap_duoi")
                        .HasForeignKey("dieu_huong_cap_tren_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_chinh_sua")
                        .WithMany()
                        .HasForeignKey("nguoi_chinh_sua_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_tao")
                        .WithMany()
                        .HasForeignKey("nguoi_tao_id");

                    b.Navigation("dieu_huong_cap_tren");

                    b.Navigation("nguoi_chinh_sua");

                    b.Navigation("nguoi_tao");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NguoiDung_2_NhomNguoiDung", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_dung")
                        .WithMany("ds_nhom_nguoi_dung")
                        .HasForeignKey("nguoi_dung_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Games.Data.Models.QTHT.NhomNguoiDung", "nhom_nguoi_dung")
                        .WithMany("ds_nguoi_dung")
                        .HasForeignKey("nhom_nguoi_dung_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("nguoi_dung");

                    b.Navigation("nhom_nguoi_dung");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NhomNguoiDung", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_chinh_sua")
                        .WithMany()
                        .HasForeignKey("nguoi_chinh_sua_id");

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", "nguoi_tao")
                        .WithMany()
                        .HasForeignKey("nguoi_tao_id");

                    b.Navigation("nguoi_chinh_sua");

                    b.Navigation("nguoi_tao");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NhomNguoiDung_2_DieuHuong", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.DieuHuong", "dieu_huong")
                        .WithMany("ds_nhom_nguoi_dung")
                        .HasForeignKey("dieu_huong_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Games.Data.Models.QTHT.NhomNguoiDung", "nhom_nguoi_dung")
                        .WithMany("ds_dieu_huong")
                        .HasForeignKey("nhom_nguoi_dung_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("dieu_huong");

                    b.Navigation("nhom_nguoi_dung");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Games.Data.Models.QTHT.NguoiDung", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Games.Data.Models.ChucNang.Categories", b =>
                {
                    b.Navigation("ds_categories_cap_duoi");

                    b.Navigation("ds_games");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.DieuHuong", b =>
                {
                    b.Navigation("ds_dieu_huong_cap_duoi");

                    b.Navigation("ds_nhom_nguoi_dung");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NguoiDung", b =>
                {
                    b.Navigation("ds_nhom_nguoi_dung");
                });

            modelBuilder.Entity("Games.Data.Models.QTHT.NhomNguoiDung", b =>
                {
                    b.Navigation("ds_dieu_huong");

                    b.Navigation("ds_nguoi_dung");
                });
#pragma warning restore 612, 618
        }
    }
}
