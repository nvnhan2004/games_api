using Games.Data.Models.QTHT;
using Games.Services.Common;
using Games.Services.Services.Users.DTO;

namespace Games.Services.Services.Users
{
    public interface IUserService : IGenericService<NguoiDung>
    {
        UserDTO GetById(Guid id);
        void Create(UserDTO model);
        UserDTO Update(Guid id, UserDTO model);
        void Delete(Guid id);
        void Deletes(List<Guid> ids);
        List<string> GetPermissionUser(Guid id);
        List<NguoiDungSelectDTO> GetAllNguoiDung();
    }
}
