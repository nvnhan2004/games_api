using AutoMapper;
using Games.Data.Models;
using Games.Data.Models.QTHT;
using Games.Services.Models;
using Games.Services.Services.Users.DTO;

namespace Games.Services.Services.DieuHuong.DTO
{
    public class DieuHuongDTO : BaseDTO
    {
        public string ma { get; set; }
        public string ten { get; set; }
        public string duong_dan { get; set; }
        public string? icon { get; set; }
        public int? so_thu_tu { get; set; }
        public string? stt_order { get; set; }
        public string? mo_ta { get; set; }
        public string? ten_dieu_huong_cap_tren { get; set; }
        public Guid? dieu_huong_cap_tren_id { get; set; }
        public int cap_dieu_huong { get; set; }
        public string? muc_luc { get; set; }
        public bool super_admin { get; set; }
        public bool is_router { get; set; }
        public bool? is_show { get; set; } = true;
        public bool? is_disabled { get; set; } = false;
        public bool? is_dady { get; set; }
        public SelectDieuHuongCapTrenDTO? dieu_huong_cap_tren { get; set; }
        public List<DieuHuongDTO>? ds_dieu_huong_cap_duoi { get; set; }
        public List<TreeDieuHuongDTO>? tree_dieu_huong { get; set; }
        public List<Data.Models.QTHT.NhomNguoiDung>? ds_nhom_nguoi_dung { get; set; }
    }
    public class DieuHuongProfile : Profile
    {
        public DieuHuongProfile()
        {
            CreateMap<Data.Models.QTHT.DieuHuong, DieuHuongDTO>()
                .ForMember(d => d.ds_nhom_nguoi_dung, opt => opt.Ignore())
                .ForMember(x => x.dieu_huong_cap_tren, otp => otp.MapFrom(z => new SelectDieuHuongCapTrenDTO
                {
                    value = z.dieu_huong_cap_tren_id != null ? z.dieu_huong_cap_tren.id : new Guid(),
                    label = z.dieu_huong_cap_tren_id != null ? z.dieu_huong_cap_tren.ten : null
                }))
                .ForMember(x => x.ten_dieu_huong_cap_tren, otp => otp.MapFrom(z => z.dieu_huong_cap_tren_id != null ? z.dieu_huong_cap_tren.ten : null))
                .ForMember(x => x.ds_dieu_huong_cap_duoi, otp => otp.Ignore())
                .ForMember(x => x.tree_dieu_huong, otp => otp.Ignore())
                .ForMember(x => x.is_dady, otp => otp.MapFrom(z => z.ds_dieu_huong_cap_duoi.Any() ? true : false))
                .ForMember(x => x.is_show, opt => opt.Ignore())
                .ForMember(x => x.is_disabled, opt => opt.Ignore());
            CreateMap<Data.Models.QTHT.DieuHuong, PermissionUserDTO>()
                .ForMember(d => d.ma_quyen, opt => opt.MapFrom(z => z.ma));
            CreateMap<DieuHuongDTO, Data.Models.QTHT.DieuHuong>()
                .IncludeBase<BaseDTO, BaseModel>()
                .ForMember(x => x.muc_luc, opt => opt.Ignore())
                .ForMember(x => x.stt_order, opt => opt.Ignore())
                .ForMember(x => x.ds_nhom_nguoi_dung, opt => opt.Ignore())
                .ForMember(x => x.dieu_huong_cap_tren, otp => otp.Ignore())
                .ForMember(x => x.ds_dieu_huong_cap_duoi, otp => otp.Ignore());

        }
    }
    public class SelectDieuHuongCapTrenDTO
    {
        public Guid? value { get; set; }
        public string? label { get; set; }
    }

    public class TreeDieuHuongDTO
    {
        public Guid value { get; set; }
        public string ma { get; set; }
        public string label { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int? so_thu_tu { get; set; }
        public string stt_order { get; set; }
        public List<TreeDieuHuongDTO> children { get; set; }
    }

    public class ItemDieuHuong
    {
        public string muc_luc { get; set; }
        public string ten_day_du { get; set; }
        public int cap_dieu_huong { get; set; }
    }

    public class MaDieuHuongDTO
    {
        public string ma { get; set; }
        public string ma_child { get; set; }
    }

    public class DieuHuongSelectDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }

    public class PhanLoaiDieuHuongDTO
    {
        public List<TreeDieuHuongDTO> lts_quan_tri { get; set; }
        public List<TreeDieuHuongDTO> lts_nghiep_vu { get; set; }
    }
}
