using Games.Services.Common;
using Games.Services.Services.NhomNguoiDung.DTO;

namespace Games.Services.Services.NhomNguoiDung
{
    public interface INhomNguoiDungService : IGenericService<Data.Models.QTHT.NhomNguoiDung>
    {
        bool AddUserToGroup(UpdateNguoiDungFromNhomND obj);
        bool AddPermission(AddPermission obj);

    }
}
