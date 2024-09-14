using Games.Data.Models.QTHT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Games.Data.Models
{
    public partial class AppDbContext : IdentityDbContext<NguoiDung, Roles, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);

            ModelsBuilderQTHT.OnModelCreating(builder);

            ModelsBuilderCN.OnModelCreating(builder);


            builder.Entity<NguoiDung>(b =>
            {
                b.Property(e => e.Id).HasColumnName("id");
                b.Property(e => e.UserName).HasColumnName("username");
                b.Property(e => e.Email).HasColumnName("email");
                b.Property(e => e.PhoneNumber).HasColumnName("phonenumber");
            });
            base.OnModelCreating(builder);
        }
    }
}
