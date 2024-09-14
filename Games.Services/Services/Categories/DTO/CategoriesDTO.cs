using AutoMapper;
using Games.Data.Models;
using Games.Services.Models;
using Games.Services.Services.Games.DTO;
using System.ComponentModel.DataAnnotations;

namespace Games.Services.Services.Categories.DTO
{
    public class CategoriesDTO : BaseDTO
    {
        public string ten { get; set; }
        public string? mo_ta { get; set; }
        public bool? su_dung { get; set; }
        public int? so_thu_tu { get; set; }
        public string? stt_order { get; set; }
        public string? muc_luc { get; set; }
        public int cap { get; set; }
        public Guid? categories_cap_tren_id { get; set; }
        public string? ten_categories_cap_tren { get; set; }
        public bool? is_show { get; set; } = true;
        public string? img { get; set; }
        public string? url_crawl { get; set; }
        public int? total_game { get; set; }
        public int? total_error_game { get; set; }
        public bool? is_crawl { get; set; }
        public bool? is_menu { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? slug { get; set; }
        public SelectCategoriesCapTrenDTO? categories_cap_tren { get; set; }
        public List<CategoriesDTO>? ds_categories_cap_duoi { get; set; }
        public List<TreeCategoriesDTO>? tree_categories { get; set; }
        public List<GamesDTO>? ds_games { get; set; }
    }
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Data.Models.ChucNang.Categories, CategoriesDTO>()
                .ForMember(x => x.ds_games, otp => otp.Ignore())
                .ForMember(x => x.categories_cap_tren, otp => otp.MapFrom(z => new SelectCategoriesCapTrenDTO
                {
                    value = z.categories_cap_tren_id != null ? z.categories_cap_tren.id : new Guid(),
                    label = z.categories_cap_tren_id != null ? z.categories_cap_tren.ten : null
                }))
                .ForMember(x => x.ten_categories_cap_tren, otp => otp.MapFrom(z => z.categories_cap_tren_id != null ? z.categories_cap_tren.ten : null))
                .ForMember(x => x.ds_categories_cap_duoi, otp => otp.Ignore())
                .ForMember(x => x.is_show, opt => opt.Ignore())
                .ForMember(x => x.tree_categories, otp => otp.Ignore());
            CreateMap<CategoriesDTO, Data.Models.ChucNang.Categories>()
                .IncludeBase<BaseDTO, BaseModel>()
                .ForMember(x => x.muc_luc, opt => opt.Ignore())
                .ForMember(x => x.stt_order, opt => opt.Ignore())
                .ForMember(x => x.categories_cap_tren, otp => otp.Ignore())
                .ForMember(x => x.ds_categories_cap_duoi, otp => otp.Ignore())
                .ForMember(x => x.ds_games, otp => otp.Ignore());

        }
    }
    public class SelectCategoriesCapTrenDTO
    {
        public Guid? value { get; set; }
        public string? label { get; set; }
    }

    public class TreeCategoriesDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public int? so_thu_tu { get; set; }
        public string stt_order { get; set; }
        public string? slug { get; set; }
        public List<TreeCategoriesDTO> children { get; set; }
    }

    public class TreeCategoriesPublicDTO
    {
        public Guid id { get; set; }
        public string ten { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? so_thu_tu { get; set; }
        public string? img { get; set; }
        public string? slug { get; set; }
        public List<GamesPublicDTO>? ds_games { get; set; }
        public List<TreeCategoriesPublicDTO> children { get; set; }
    }

    public class ItemCategories
    {
        public string muc_luc { get; set; }
        public string ten_day_du { get; set; }
        public int cap { get; set; }
    }
    public class CategoriesConfiguarationDTO
    {
        public string url_crawl { get; set; }
    }
}
