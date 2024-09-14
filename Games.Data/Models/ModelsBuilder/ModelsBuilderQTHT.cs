using Games.Data.Models.QTHT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Games.Data.Models
{
    public partial class AppDbContext : IdentityDbContext<NguoiDung, Roles, Guid>
    {

        public DbSet<NhomNguoiDung> NhomNguoiDung { get; set; }
        public DbSet<DieuHuong> DieuHuong { get; set; }
        public DbSet<NhomNguoiDung_2_DieuHuong> NhomNguoiDung_2_DieuHuong { get; set; }
        public DbSet<NguoiDung_2_NhomNguoiDung> NguoiDung_2_NhomNguoiDung { get; set; }
    }

    public static class ModelsBuilderQTHT
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region nhóm người dùng
            modelBuilder.Entity<NhomNguoiDung>()
                .HasOne(e => e.nguoi_tao)
                .WithMany()
                .HasForeignKey(e => e.nguoi_tao_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<NhomNguoiDung>()
                .HasOne(e => e.nguoi_chinh_sua)
                .WithMany()
                .HasForeignKey(e => e.nguoi_chinh_sua_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region NguoiDung_2_NhomNguoiDung
            modelBuilder.Entity<NguoiDung_2_NhomNguoiDung>()
                .HasKey(e => new { e.nguoi_dung_id, e.nhom_nguoi_dung_id });
            modelBuilder.Entity<NguoiDung_2_NhomNguoiDung>()
                .HasOne(e => e.nguoi_dung)
                .WithMany(e => e.ds_nhom_nguoi_dung)
                .HasForeignKey(e => e.nguoi_dung_id)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NguoiDung_2_NhomNguoiDung>()
                .HasOne(e => e.nhom_nguoi_dung)
                .WithMany(e => e.ds_nguoi_dung)
                .HasForeignKey(e => e.nhom_nguoi_dung_id)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region NhomNguoiDung_2_DieuHuong
            modelBuilder.Entity<NhomNguoiDung_2_DieuHuong>()
                .HasKey(e => new { e.nhom_nguoi_dung_id, e.dieu_huong_id });
            modelBuilder.Entity<NhomNguoiDung_2_DieuHuong>()
                .HasOne(e => e.nhom_nguoi_dung)
                .WithMany(e => e.ds_dieu_huong)
                .HasForeignKey(e => e.nhom_nguoi_dung_id)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NhomNguoiDung_2_DieuHuong>()
                .HasOne(e => e.dieu_huong)
                .WithMany(e => e.ds_nhom_nguoi_dung)
                .HasForeignKey(e => e.dieu_huong_id)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region dieu_huong
            modelBuilder.Entity<DieuHuong>()
                .HasOne(e => e.nguoi_tao)
                .WithMany()
                .HasForeignKey(e => e.nguoi_tao_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<DieuHuong>()
                .HasOne(e => e.nguoi_chinh_sua)
                .WithMany()
                .HasForeignKey(e => e.nguoi_chinh_sua_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<DieuHuong>()
                 .HasOne(e => e.dieu_huong_cap_tren)
                 .WithMany(e => e.ds_dieu_huong_cap_duoi)
                 .HasForeignKey(e => e.dieu_huong_cap_tren_id)
                 .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion
        }
    }
}
