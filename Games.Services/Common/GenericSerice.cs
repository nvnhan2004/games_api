using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using Games.Data.Models;
using Games.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Claims;
using System.Linq;

namespace Games.Services.Common
{
    public abstract class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        protected AppDbContext _dbContext;
        protected DbSet<TEntity> _repo;
        protected MapperConfiguration _mapperCfg;
        protected IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericService(AppDbContext dbContext = null, IHttpContextAccessor httpContextAccessor = null)
        {
            if (dbContext != null)
            {
                this._dbContext = dbContext;
            }
            else
            {
                this._dbContext = new AppDbContext(new DbContextOptions<AppDbContext>());
            }
            this._repo = this._dbContext.Set<TEntity>();
            _httpContextAccessor = httpContextAccessor;
        }
        #region CRUD
        public virtual async Task<TDto> CreateAsync<TDto>(TDto dto, IMapper mapper = null) where TDto : class
        {
            try
            {
                mapper = GetMapper<TDto>(mapper);
                var entity = await AddToDbAsync<TDto>(dto, mapper);
                if (_dbContext.SaveChanges() >= 1)
                {
                    AfferSaveChange(true, ref dto, ref entity);
                    // _repo.ReloadRef(entity);
                    return mapper.Map<TEntity, TDto>(entity);
                }
                else
                {
                    ErrorCtr.Reject(HttpStatusCode.BadRequest, "invalid_request", "Có lỗi xảy ra trong quá trình lưu");
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual async Task<TDto> UpdateAsync<TDto>(TDto dto, Guid id, IMapper mapper = null) where TDto : class
        {
            try
            {
                mapper = GetMapper<TDto>(mapper);
                var entity = await UpdateToDbAsync<TDto>(dto, id, mapper);
                if (entity == null)
                {
                    ErrorCtr.NotFound();
                    return null;
                }
                if (_dbContext.SaveChanges() >= 1)
                {
                    AfferSaveChange(false, ref dto, ref entity);
                    // _repo.ReloadRef(entity);
                    return mapper.Map<TEntity, TDto>(entity);
                }
                else
                {
                    ErrorCtr.Reject(HttpStatusCode.BadRequest, "invalid_request", "Có lỗi xảy ra trong quá trình cập nhật");
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual async Task<TDto> GetByIdAsync<TDto>(Guid id, IMapper mapper = null) where TDto : class
        {
            try
            {
                var entity = await _repo.FindAsync(id);
                if (entity != null)
                {
                    mapper = GetMapper<TDto>(mapper);
                    var x = mapper.Map<TEntity, TDto>(entity);
                    return x;
                }
                else
                {
                    ErrorCtr.NotFound();
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<ResponseList<TDto>> GetManyAsync<TDto>(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null, IMapper mapper = null) where TDto : class
        {
            mapper = GetMapper<TDto>(mapper);
            int total = 0;
            var query = this.eGetMany(page, page_size, sort, filter, search, ref total);
            var result = await query.ProjectTo<TDto>(mapper.ConfigurationProvider).ToListAsync();
            return new ResponseList<TDto>(new Meta(page, page_size, total), result);
        }
        public virtual async Task<ResponseList<TDto>> GetManyMapAsync<TDto>(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null, IMapper mapper = null) where TDto : class
        {
            mapper = GetMapper<TDto>(mapper);
            int total = 0;
            var query = this.eGetMany(page, page_size, sort, filter, search, ref total);
            var list = await query.ToListAsync();
            var result = mapper.Map<List<TEntity>, List<TDto>>(list);
            return new ResponseList<TDto>(new Meta(page, page_size, total), result);
        }
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                BeforeDelete(id);
                return await _repo.Where("x => id == @0", id).BatchDeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual async Task<int> DeletesAsync(List<Guid> ids)
        {
            return await _repo.Where("x => @0.Contains(id)", ids).BatchDeleteAsync();
        }
        public virtual async Task<int> DeletesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repo.Where(predicate).BatchDeleteAsync();
        }
        #endregion
        #region Các function xử lý ở mức dbcontext, Queryable
        public virtual IQueryable<TEntity> eGetMany(int page, int page_size, string sort, string filter, string search, ref int total)
        {
            try
            {
                #region xử lý đầu vào sort
                Dictionary<string, dynamic> sortObject = new Dictionary<string, dynamic>();
                try
                {
                    if (sort != null)
                    {
                        sortObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(sort);
                    }
                }
                catch (Exception)
                {
                    ErrorCtr.Reject(HttpStatusCode.BadRequest, "invalid_argument", "tham số sort truyền vào không đúng");
                }
                if (sortObject.Count() == 0)
                {
                    sortObject.Add("id", 1);
                }
                #endregion
                #region xử lý đầu vào filter
                dynamic filterObj = new object();
                try
                {
                    if (filter != null)
                    {
                        filterObj = JsonConvert.DeserializeObject<dynamic>(filter);
                    }
                    else
                    {
                        filterObj = null;
                    }
                }
                catch (Exception)
                {
                    ErrorCtr.Reject(HttpStatusCode.BadRequest, "invalid_argument", "tham số filter truyền vào không đúng");
                }
                #endregion
                var query = _repo.AsQueryable();
                #region thực hiện apply filter
                query = QueryBuilder(query, filterObj, search);
                #endregion
                #region thực hiện apply sort query
                String OrderBy = "";
                foreach (var key in sortObject.Keys)
                {
                    // check if the value is not null or empty.
                    if (sortObject.ContainsKey(key))
                    {
                        var orderProp = sortObject[key] == 1 ? "ascending" : "descending";
                        OrderBy += key + " " + orderProp + ",";
                    }
                }
                OrderBy = OrderBy[0..(OrderBy.Length - 1)];
                query = query.OrderBy(OrderBy);
                #endregion

                #region thực hiện phân trang và trả về kết quả
                total = query.Count();
                if (page > 0 && page_size > 0)
                {
                    var Index = (page - 1) * page_size;
                    query = query.Skip(Index).Take(page_size);
                }
                #endregion
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<TEntity> AddToDbAsync<TDto>(TDto dto, IMapper mapper = null) where TDto : class
        {
            try
            {
                Boolean isNew = true;
                //trước khi map DTO sang entity
                BeforeMapper<TDto>(isNew, ref dto);
                mapper = GetMapper<TDto>(mapper);
                var entity = mapper.Map<TDto, TEntity>(dto);
                //xử lý cập nhật các trường ngày tháng chỉnh sửa cho entity
                UpdateCreatedDate(isNew, ref entity);
                //Xử lý trước khi add vào DBcontext
                BeforeAddOrUpdate<TDto>(isNew, ref dto, ref entity);
                await _repo.AddAsync(entity);
                //trước khi save vào cơ sở dữ liệu
                AfferAddOrUpdate(isNew, ref dto, ref entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual async Task<TEntity> UpdateToDbAsync<TDto>(TDto dto, Guid id, IMapper mapper = null) where TDto : class
        {
            try
            {
                Boolean isNew = false;
                //trước khi map DTO sang entity
                BeforeMapper<TDto>(isNew, ref dto);
                mapper = GetMapper<TDto>(mapper);
                var entity = _repo.Find(id);
                if (entity != null)
                {
                    mapper.Map<TDto, TEntity>(dto, entity);
                    //xử lý cập nhật các trường ngày tháng chỉnh sửa cho entity
                    UpdateCreatedDate(isNew, ref entity);
                    //Xử lý trước khi add vào DBcontext
                    BeforeAddOrUpdate<TDto>(isNew, ref dto, ref entity);
                    _repo.Update(entity);
                    //trước khi save vào cơ sở dữ liệu
                    AfferAddOrUpdate(isNew, ref dto, ref entity);
                }
                return await Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Middleware 
        /// <summary>
        /// Xử lý phần sinh query cho từng services
        /// </summary>
        /// <typeparam name="Boolean"></typeparam>
        /// <param name="isNew">xác định xem có phải thêm mới hay không</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> QueryBuilder(IQueryable<TEntity> query, dynamic filter, string search)
        {
            return query;
        }
        /// <summary>
        /// Xử lý trước khi map từ dto vào entity
        /// </summary>
        /// <typeparam name="Boolean"></typeparam>
        /// <param name="isNew">xác định xem có phải thêm mới hay không</param>
        /// <returns></returns>
        protected virtual void BeforeMapper<TDto>(Boolean isNew, ref TDto dto)
        {
        }
        /// <summary>
        /// Xử lý trước khi add hoặc update vào dbcontext
        /// </summary>
        /// <typeparam name="Boolean"></typeparam>
        /// <param name="isNew">xác định xem có phải thêm mới hay không</param>
        /// <returns></returns>
        protected virtual void BeforeAddOrUpdate<TDto>(Boolean isNew, ref TDto dto, ref TEntity entity)
        {
        }
        /// <summary>
        /// Xử lý sau khi add hoặc update vào dbcontext
        /// </summary>
        /// <typeparam name="Boolean"></typeparam>
        /// <param name="isNew">xác định xem có phải thêm mới hay không</param>
        /// <returns></returns>
        protected virtual void AfferAddOrUpdate<TDto>(Boolean isNew, ref TDto dto, ref TEntity entity)
        {


        }

        /// <summary>
        /// Xử lý sau khi save change vào csdl dbcontext
        /// </summary>
        /// <typeparam name="Boolean"></typeparam>
        /// <param name="isNew">xác định xem có phải thêm mới hay không</param>
        /// <returns></returns>
        protected virtual void AfferSaveChange<TDto>(Boolean isNew, ref TDto dto, ref TEntity entity)
        {
        }


        protected virtual void BeforeDelete(Guid id)
        {

        }
        #endregion
        private void UpdateCreatedDate(Boolean isNew, ref TEntity entity)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var id = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid)
               .Select(c => c.Value).SingleOrDefault();

            var created_date = entity.GetType().GetProperty("ngay_tao");
            var modified_date = entity.GetType().GetProperty("ngay_chinh_sua");
            var created_uid = entity.GetType().GetProperty("nguoi_tao_id");
            var modified_uid = entity.GetType().GetProperty("nguoi_chinh_sua_id");
            if (isNew)
            {
                if (created_date != null)
                {
                    created_date.SetValue(entity, DateTime.Now);
                }
                if (created_uid != null && currentUser != null)
                {
                    created_uid.SetValue(entity, Guid.Parse(id));
                }
            }
            if (modified_date != null)
            {
                modified_date.SetValue(entity, DateTime.Now);
            }
            if (modified_uid != null && currentUser != null)
            {
                modified_uid.SetValue(entity, Guid.Parse(id));
            }
        }

        private IMapper GetMapper<TDto>(IMapper mapper = null)
        {

            if (mapper == null && _mapper == null)
            {
                var mapperCfg = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BaseProfile>();
                    cfg.CreateMap<TEntity, TDto>();
                    cfg.CreateMap<TDto, TEntity>()
                    .IncludeBase<BaseDTO, Data.Models.BaseModel>();
                });
                mapper = mapperCfg.CreateMapper();
                mapperCfg.AssertConfigurationIsValid();
            }
            else if (mapper == null)
            {
                mapper = _mapper;
            }
            return mapper;
        }
    }
}
