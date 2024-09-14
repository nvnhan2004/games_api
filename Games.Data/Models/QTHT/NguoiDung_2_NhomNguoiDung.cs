using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.QTHT
{
    [Table("qtht_nguoi_dung_2_nhom_nguoi_dung")]
    public class NguoiDung_2_NhomNguoiDung
    {
        public NguoiDung_2_NhomNguoiDung()
        {
        }

        public Guid nguoi_dung_id { get; set; }
        public virtual NguoiDung nguoi_dung { get; set; }
        public Guid nhom_nguoi_dung_id { get; set; }
        public virtual NhomNguoiDung nhom_nguoi_dung { get; set; }
    }
}
