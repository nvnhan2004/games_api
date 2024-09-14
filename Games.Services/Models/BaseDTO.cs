using AutoMapper;
using Games.Data.Models;
using Games.Data.Models.QTHT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Services.Models
{
    public class BaseDTO
    {
        public Guid id { get; set; }
        public Guid? nguoi_tao_id { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
        public NguoiTaoDTO? nguoi_tao { get; set; }
        public NguoiChinhSuaDTO? nguoi_chinh_sua { get; set; }
    }
    public class NguoiTaoDTO
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string? full_name { get; set; }
    }
    public class NguoiChinhSuaDTO
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string? full_name { get; set; }
    }
    public class CurrentUserDTO
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string? full_name { get; set; }
        public bool super_admin { get; set; }
        public string? email { get; set; }
    }
    public class SelectDTO
    {
        public Guid id { get; set; }
        public string ten { get; set; }
    }
    public class SelectSimpleDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
        public int? loai { get; set; }
    }
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<NguoiDung, NguoiChinhSuaDTO>();
            CreateMap<NguoiDung, NguoiTaoDTO>();
            CreateMap<BaseDTO, BaseModel>()
            .ForMember(d => d.nguoi_tao, opt => opt.Ignore())
            .ForMember(d => d.nguoi_tao_id, opt => opt.Ignore())
            .ForMember(d => d.nguoi_chinh_sua_id, opt => opt.Ignore())
            .ForMember(d => d.ngay_tao, opt => opt.Ignore())
            .ForMember(d => d.ngay_chinh_sua, opt => opt.Ignore())
            .ForMember(d => d.nguoi_chinh_sua, opt => opt.Ignore());
        }
    }
}
