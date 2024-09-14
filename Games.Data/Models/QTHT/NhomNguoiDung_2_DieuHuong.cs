using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.QTHT
{
    [Table("qtht_nhom_nguoi_dung_2_dieu_huong")]
    public class NhomNguoiDung_2_DieuHuong
    {
        public NhomNguoiDung_2_DieuHuong()
        {
        }

        public Guid nhom_nguoi_dung_id { get; set; }
        public virtual NhomNguoiDung nhom_nguoi_dung { get; set; }
        public Guid dieu_huong_id { get; set; }
        public virtual DieuHuong dieu_huong { get; set; }
    }
}
