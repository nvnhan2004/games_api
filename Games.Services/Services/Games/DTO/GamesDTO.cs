using AutoMapper;
using Games.Data.Models;
using Games.Services.Models;

namespace Games.Services.Services.Games.DTO
{
    public class GamesDTO : BaseDTO
    {
        public string ten { get; set; }
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
        public string? title { get; set; }
        public string? description { get; set; }
        public string? slug { get; set; }
        public Guid category_id { get; set; }
        public string? ten_category { get; set; }
    }

    public class GamesProfile : Profile
    {
        public GamesProfile()
        {
            CreateMap<Data.Models.ChucNang.Games, GamesDTO>().IncludeAllDerived()
                .ForMember(x => x.ten_category, otp => otp.MapFrom(z => z.categories != null ? z.categories.ten : ""));
            CreateMap<GamesDTO, Data.Models.ChucNang.Games>()
                .ForMember(x => x.categories, opt => opt.Ignore())
                .IncludeBase<BaseDTO, BaseModel>();

            CreateMap<Data.Models.ChucNang.Games, GamesPublicDTO>().IncludeAllDerived()
                .ForMember(x => x.ten_category, otp => otp.MapFrom(z => z.categories != null ? z.categories.ten : ""));
        }
    }
    public class LtsGamesDTO : BaseDTO
    {
        public string ten { get; set; }
        public string? img { get; set; }
        public bool? is_trending { get; set; }
        public bool? is_new { get; set; }
        public bool? is_menu { get; set; }
        public Guid category_id { get; set; }
        public string? ten_category { get; set; }
    }
    public class GamesPublicDTO
    {
        public Guid id { get; set; }
        public string ten { get; set; }
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
        public string? title { get; set; }
        public string? description { get; set; }
        public string? slug { get; set; }
        public Guid category_id { get; set; }
        public string? ten_category { get; set; }
    }
}
