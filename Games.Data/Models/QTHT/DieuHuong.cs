using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.QTHT
{
    [Table("qtht_dieu_huong")]
    public class DieuHuong : BaseModel
    {
        public DieuHuong()
        : base()
        {
            this.ds_nhom_nguoi_dung = new HashSet<NhomNguoiDung_2_DieuHuong>();
        }
        [Required]
        [StringLength(32)]
        public string ma { get; set; }

        [Required]
        public string ten { get; set; }

        [StringLength(255)]
        public string duong_dan { get; set; }

        [StringLength(255)]
        public string? icon { get; set; }

        public int? so_thu_tu { get; set; }
        public string stt_order { get; set; }

        public string? mo_ta { get; set; }

        public int cap_dieu_huong { get; set; }

        [StringLength(1024)]
        public string muc_luc { get; set; }

        public Guid? dieu_huong_cap_tren_id { get; set; }

        public bool? super_admin { get; set; }

        public bool is_router { get; set; }
        public virtual DieuHuong dieu_huong_cap_tren { get; set; }

        public virtual ICollection<DieuHuong> ds_dieu_huong_cap_duoi { get; set; }
        public virtual ICollection<NhomNguoiDung_2_DieuHuong> ds_nhom_nguoi_dung { get; set; }
    }
}
