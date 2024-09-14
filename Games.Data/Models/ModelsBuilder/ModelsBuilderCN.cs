using Games.Data.Models.ChucNang;
using Games.Data.Models.QTHT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Games.Data.Models
{
    public partial class AppDbContext : IdentityDbContext<NguoiDung, Roles, Guid>
    {
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ChucNang.Games> Games { get; set; }
    }

    public static class ModelsBuilderCN
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Categories
            modelBuilder.Entity<Categories>()
                .HasOne(e => e.nguoi_tao)
                .WithMany()
                .HasForeignKey(e => e.nguoi_tao_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Categories>()
                .HasOne(e => e.nguoi_chinh_sua)
                .WithMany()
                .HasForeignKey(e => e.nguoi_chinh_sua_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Categories>()
                 .HasOne(e => e.categories_cap_tren)
                 .WithMany(e => e.ds_categories_cap_duoi)
                 .HasForeignKey(e => e.categories_cap_tren_id)
                 .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region Games
            modelBuilder.Entity<ChucNang.Games>()
                .HasOne(e => e.nguoi_tao)
                .WithMany()
                .HasForeignKey(e => e.nguoi_tao_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<ChucNang.Games>()
                .HasOne(e => e.nguoi_chinh_sua)
                .WithMany()
                .HasForeignKey(e => e.nguoi_chinh_sua_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<ChucNang.Games>()
                 .HasOne(e => e.categories)
                 .WithMany(e => e.ds_games)
                 .HasForeignKey(e => e.category_id)
                 .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion
        }
    }
}
