using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.QTHT
{
    [Table("qtht_nguoi_dung")]
    public class NguoiDung : IdentityUser<Guid>
    {
        public NguoiDung() : base()
        {
            {
                this.ds_nhom_nguoi_dung = new HashSet<NguoiDung_2_NhomNguoiDung>();
            }
        }
        public string? full_name { get; set; }
        public bool? super_admin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public Guid? nguoi_tao_id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
        //public virtual NguoiDung nguoi_tao { get; set; }
        //public virtual NguoiDung nguoi_chinh_sua { get; set; }
        public virtual ICollection<NguoiDung_2_NhomNguoiDung> ds_nhom_nguoi_dung { get; set; }
    }
}
