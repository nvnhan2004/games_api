using Games.Data.Models.QTHT;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.Data.Models.ChucNang
{
    [Table("cn_categories")]
    public class Categories : BaseModel
    {
        public Categories() : base()
        {
            this.ds_games = new HashSet<Games>();
        }
        [StringLength(512)]
        public string ten { get; set; }
        public string? mo_ta { get; set; }
        public bool? su_dung { get; set; }
        public int? so_thu_tu { get; set; }
        public string? stt_order { get; set; }

        [StringLength(512)]
        public string? url_crawl { get; set; }
        public int? total_game { get; set; }
        public int? total_error_game { get; set; }
        public bool? is_crawl { get; set; }
        public bool? is_menu { get; set; }

        [StringLength(1024)]
        public string? title { get; set; }

        [StringLength(4096)]
        public string? description { get; set; }

        [StringLength(1024)]
        public string? img { get; set; }
        [StringLength(512)]
        public string? slug { get; set; }
        public int cap { get; set; }

        [StringLength(1024)]
        public string muc_luc { get; set; }
        public Guid? categories_cap_tren_id { get; set; }
        public virtual Categories categories_cap_tren { get; set; }
        public virtual ICollection<Categories> ds_categories_cap_duoi { get; set; }
        public virtual ICollection<Games> ds_games { get; set; }
    }
}
