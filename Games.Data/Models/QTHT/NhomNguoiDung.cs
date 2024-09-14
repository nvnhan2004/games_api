using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.QTHT
{
    [Table("qtht_nhom_nguoi_dung")]
    public class NhomNguoiDung : BaseModel
    {
        public NhomNguoiDung()
        : base()
        {
            this.ds_nguoi_dung = new HashSet<NguoiDung_2_NhomNguoiDung>();
            this.ds_dieu_huong = new HashSet<NhomNguoiDung_2_DieuHuong>();
        }

        [Required]
        [StringLength(255)]
        public string ma { get; set; }

        [Required]
        public string ten { get; set; }

        public string? mota { get; set; }
        public virtual ICollection<NguoiDung_2_NhomNguoiDung> ds_nguoi_dung { get; set; }
        public virtual ICollection<NhomNguoiDung_2_DieuHuong> ds_dieu_huong { get; set; }
    }
}
