using AutoMapper;
using Games.Data.Models.QTHT;

namespace Games.Services.Services.Users.DTO
{
    public class UserDTO
    {
        public Guid id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? phonenumber { get; set; }
        public string? full_name { get; set; }

    }

    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<NguoiDung, UserDTO>().IncludeAllDerived()
             .ForMember(x => x.id, otp => otp.MapFrom(z => z.Id))
             .ForMember(x => x.username, otp => otp.MapFrom(z => z.UserName))
             .ForMember(x => x.full_name, otp => otp.MapFrom(z => z.full_name))
             .ForMember(x => x.email, otp => otp.MapFrom(z => z.Email))
             .ForMember(x => x.phonenumber, otp => otp.MapFrom(z => z.PhoneNumber));
            CreateMap<NguoiDung, NguoiDungSelectDTO>()
                .ForMember(x => x.value, otp => otp.MapFrom(z => z.Id))
                .ForMember(x => x.label, otp => otp.MapFrom(z => z.full_name));
        }
    }
    public class UpdateNhomNDFromND
    {
        public Guid nguoi_dung_id { get; set; }
        public List<Data.Models.QTHT.NhomNguoiDung> lts_nhom_id { get; set; }
    }

    public class PermissionUserDTO
    {
        public string ma_quyen { get; set; }
    }

    public class NguoiDungSelectDTO
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }
    public class NguoiDungEmailSelectDTO
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class NguoiDungNND_DTO
    {
        public string username { get; set; }
        public Guid id { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
    }
}
