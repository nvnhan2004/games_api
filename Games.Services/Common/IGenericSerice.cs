using AutoMapper;
using Games.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Games.Services.Common
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Lấy danh sách bản ghi theo thông số truyền vào
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseList<TDto>> GetManyAsync<TDto>(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null, IMapper mapper = null) where TDto : class;
        /// <summary>
        /// Lấy danh sách bản ghi theo thông số truyền vào
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseList<TDto>> GetManyMapAsync<TDto>(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null, IMapper mapper = null) where TDto : class;

        /// <summary>
        /// Lấy thông tin chi tiết bản ghi
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TDto> GetByIdAsync<TDto>(Guid id, IMapper mapper = null) where TDto : class;

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<TDto> CreateAsync<TDto>(TDto dto, IMapper mapper = null) where TDto : class;


        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDto> UpdateAsync<TDto>(TDto dto, Guid id, IMapper mapper = null) where TDto : class;

        /// <summary>
        /// Xóa 1 dữ liệu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Xóa nhiều dữ liệu theo ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(List<Guid> ids);

        Task<int> DeletesAsync(Expression<Func<TEntity, bool>> predicate);


        IQueryable<TEntity> eGetMany(int page, int page_size, string sort, string filter, string search, ref int total);
        Task<TEntity> AddToDbAsync<TDto>(TDto dto, IMapper mapper = null) where TDto : class;
        Task<TEntity> UpdateToDbAsync<TDto>(TDto dto, Guid id, IMapper mapper = null) where TDto : class;
    }
}
