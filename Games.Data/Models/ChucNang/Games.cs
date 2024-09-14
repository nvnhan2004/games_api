using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.ChucNang
{
    [Table("cn_games")]
    public class Games : BaseModel
    {
        public Games() : base()
        {

        }
        [StringLength(512)]
        public string ten { get; set; }

        [StringLength(2048)]
        public string? iframe { get; set; }
        public string? mo_ta { get; set; }
        public bool? su_dung { get; set; }
        public int? so_thu_tu { get; set; }
        public int? so_lan_choi { get; set; }
        public int? like { get; set; }
        public int? dislike { get; set; }
        public string? img { get; set; }
        public bool? is_trending { get; set; }
        public bool? is_new { get; set; }
        public bool? is_menu { get; set; }

        [StringLength(1024)]
        public string? title { get; set; }

        [StringLength(4096)]
        public string? description { get; set; }
        [StringLength(512)]
        public string? slug { get; set; }
        public Guid category_id { get; set; }
        public virtual Categories categories { get; set; }
    }
}
