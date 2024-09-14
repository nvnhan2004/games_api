using Games.Services.Common;
using Games.Services.Services.Categories.DTO;

namespace Games.Services.Services.Categories
{
    public interface ICategoriesService : IGenericService<Data.Models.ChucNang.Categories>
    {
        List<TreeCategoriesDTO> GetTreeCategories();
        List<TreeCategoriesDTO> GetTreeCategoriesForm();
        List<CategoriesDTO> GetFlatCategories();
        bool DeleteByMucLuc(Guid id);
        void UpdateSttOrder();
        int? GetStt(Guid? categories_cap_tren_id);
        string CrawlCategories();
        List<TreeCategoriesPublicDTO> GetCategoriesMenu();
        TreeCategoriesPublicDTO GetDetailCategories(string slug);
    }
}
