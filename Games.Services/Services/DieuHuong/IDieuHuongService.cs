using Games.Services.Common;
using Games.Services.Services.DieuHuong.DTO;

namespace Games.Services.Services.DieuHuong
{
    public interface IDieuHuongService : IGenericService<Data.Models.QTHT.DieuHuong>
    {
        List<TreeDieuHuongDTO> GetTreeDieuHuong();
        List<TreeDieuHuongDTO> GetTreeDieuHuongForm();
        List<DieuHuongDTO> GetFlatDieuHuong();
        bool DeleteByMucLuc(Guid id);
        void UpdateSttOrder();
        int? GetStt(Guid? dieu_huong_cap_tren_id);
    }
}
