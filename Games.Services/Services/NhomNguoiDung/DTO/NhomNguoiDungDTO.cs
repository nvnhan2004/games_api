using AutoMapper;
using Games.Data.Models;
using Games.Services.Models;
using Games.Services.Services.DieuHuong.DTO;
using Games.Services.Services.Users.DTO;

namespace Games.Services.Services.NhomNguoiDung.DTO
{
    public class NhomNguoiDungDTO : BaseDTO
    {
        public string ma { get; set; }
        public string ten { get; set; }
        public string mota { get; set; }
        public Guid? value { get; set; }
        public string label { get; set; }
        //public List<NguoiDung_SelectDTO> ds_nguoidung_1 { get; set; }
        public List<NguoiDungNND_DTO> ds_nguoidung { get; set; }
        public List<DieuHuongDTO> ds_dieuhuong { get; set; }
    }
    public class NhomNguoiDungProfile : Profile
    {
        public NhomNguoiDungProfile()
        {

            CreateMap<Data.Models.QTHT.NhomNguoiDung, NhomNguoiDungDTO>()
            //.ForMember(x => x.ds_nguoidung_1, otp => otp.Ignore())
            .ForMember(x => x.ds_nguoidung, otp => otp.Ignore())
             .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
             .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
            .AfterMap((s, d) =>
            {
                if (s.ds_nguoi_dung.Any())
                {
                    d.ds_nguoidung = s.ds_nguoi_dung.Select(x => new NguoiDungNND_DTO
                    {
                        id = x.nguoi_dung != null ? x.nguoi_dung.Id : new Guid(),
                        username = x.nguoi_dung?.UserName,
                        email = x.nguoi_dung?.Email,
                        phonenumber = x.nguoi_dung?.PhoneNumber
                    }).ToList();
                }
                else
                {
                    d.ds_nguoidung = new List<NguoiDungNND_DTO>();
                }
            })
            .ForMember(d => d.ds_dieuhuong, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (s.ds_dieu_huong.Any())
                {
                    d.ds_dieuhuong = s.ds_dieu_huong.Select(x => new DieuHuongDTO
                    {
                        id = x.dieu_huong != null ? x.dieu_huong.id : new Guid(),
                        ma = x.dieu_huong?.ma,
                        ten = x.dieu_huong?.ten,
                        so_thu_tu = x.dieu_huong?.so_thu_tu,
                        cap_dieu_huong = x.dieu_huong?.cap_dieu_huong ?? 1,
                        muc_luc = x.dieu_huong?.muc_luc,
                        dieu_huong_cap_tren_id = x.dieu_huong?.dieu_huong_cap_tren_id
                    }).ToList();
                }
                else
                {
                    d.ds_dieuhuong = new List<DieuHuongDTO>();
                }
            });
            CreateMap<NhomNguoiDungDTO, Data.Models.QTHT.NhomNguoiDung>()
            .IncludeBase<BaseDTO, BaseModel>()
            .ForMember(x => x.ds_nguoi_dung, otp => otp.Ignore())
            .ForMember(x => x.ds_dieu_huong, otp => otp.Ignore());
        }
    }
    public class UpdateNguoiDungFromNhomND
    {
        public Guid nhom_id { get; set; }
        public List<Guid> lts_nguoi_dung_id { get; set; }
    }
    public class AddPermission
    {
        public Guid nhom_id { get; set; }
        public List<Guid> lts_dieu_huong_id { get; set; }
    }
    public class NhomNguoiDungSelectDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }
    public class NguoiDung_SelectDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }
}
