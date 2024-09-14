using Microsoft.AspNetCore.Identity;

namespace Games.Data.Models.QTHT
{
    public class Roles : IdentityRole<Guid>
    {
        public Guid? nguoi_tao_id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
    }
}
