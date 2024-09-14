using Games.Data.Models.QTHT;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models
{
    public partial class BaseModel
    {
        public BaseModel()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid? nguoi_tao_id { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
        public virtual NguoiDung nguoi_tao { get; set; }
        public virtual NguoiDung nguoi_chinh_sua { get; set; }
    }
}
