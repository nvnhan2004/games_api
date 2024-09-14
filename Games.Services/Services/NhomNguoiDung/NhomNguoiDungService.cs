using AutoMapper;
using Games.Data.Models;
using Games.Data.Models.QTHT;
using Games.Services.Common;
using Games.Services.Models;
using Games.Services.Services.NhomNguoiDung.DTO;
using Microsoft.AspNetCore.Http;

namespace Games.Services.Services.NhomNguoiDung
{
    public class NhomNguoiDungService : GenericService<Data.Models.QTHT.NhomNguoiDung>, INhomNguoiDungService
    {
        IHttpContextAccessor _httpContextAccessor;
        public NhomNguoiDungService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
           : base(dbContext, httpContextAccessor)
        {
            ///Khởi tạo mapperconfiuration
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
                cfg.AddProfile<NhomNguoiDungProfile>();
            });
            _mapper = _mapperCfg.CreateMapper();
            _mapperCfg.AssertConfigurationIsValid();
            _httpContextAccessor = httpContextAccessor;
        }

        protected override IQueryable<Data.Models.QTHT.NhomNguoiDung> QueryBuilder(IQueryable<Data.Models.QTHT.NhomNguoiDung> query, dynamic filter, string search)
        {
            if (filter != null)
            {
                String ma = filter.ma;
                if (!string.IsNullOrEmpty(ma))
                {
                    ma = ma.ToLower().Trim();
                    query = query.Where(x => x.ma.ToLower().Contains(ma));
                }
                String ten = filter.ten;
                if (!string.IsNullOrEmpty(ten))
                {
                    ten = ten.ToLower().Trim();
                    query = query.Where(x => x.ten.ToLower().Contains(ten));
                }
                String mota = filter.mota;
                if (!string.IsNullOrEmpty(mota))
                {
                    mota = mota.ToLower().Trim();
                    query = query.Where(x => x.mota.ToLower().Contains(mota));
                }
            }
            if (search != null && search != "")
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.ma.ToLower().Contains(search) ||
                                         x.ten.ToLower().Contains(search) ||
                                         x.mota.ToLower().Contains(search));
            }
            return query;
        }
        protected override void BeforeMapper<TDto>(bool isNew, ref TDto dto)
        {
            if (typeof(TDto) == typeof(NhomNguoiDungDTO))
            {
                NhomNguoiDungDTO quyennguoidungDTO = (NhomNguoiDungDTO)(object)dto;
                var isExits = _repo.Where(x => x.ma.ToLower() == quyennguoidungDTO.ma.ToLower() && x.id != quyennguoidungDTO.id);
                if (isExits.Count() > 0)
                {
                    ErrorCtr.DataExits("Mã nhóm đã tồn tại, vui lòng kiểm tra lại thông tin");
                }
            }
        }
        protected override void BeforeAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.QTHT.NhomNguoiDung entity)
        {
            if (typeof(TDto) != typeof(NhomNguoiDungDTO))
            {
                return;
            }
            NhomNguoiDungDTO nguoidungDTO = (NhomNguoiDungDTO)(object)dto;
            if (isNew)
            {

            }
        }
        protected override void AfferAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.QTHT.NhomNguoiDung entity)
        {
            if (typeof(TDto) != typeof(NhomNguoiDungDTO))
            {
                return;
            }
            NhomNguoiDungDTO nhomnguoidungDTO = (NhomNguoiDungDTO)(object)dto;
            if (isNew)
            {
                #region add người dùng vào bảng trung gian
                if (nhomnguoidungDTO.ds_nguoidung != null)
                {
                    foreach (var item in nhomnguoidungDTO.ds_nguoidung)
                    {
                        NguoiDung_2_NhomNguoiDung nguoi_dung_2_nhom_nguoi_dung = new NguoiDung_2_NhomNguoiDung();
                        nguoi_dung_2_nhom_nguoi_dung.nguoi_dung_id = item.id;
                        nguoi_dung_2_nhom_nguoi_dung.nhom_nguoi_dung_id = entity.id;
                        entity.ds_nguoi_dung.Add(nguoi_dung_2_nhom_nguoi_dung);
                    }
                }
                #endregion

                #region add điều hướng vào bảng trung gian
                if (nhomnguoidungDTO.ds_dieuhuong != null)
                {
                    foreach (var item in nhomnguoidungDTO.ds_dieuhuong)
                    {
                        NhomNguoiDung_2_DieuHuong nhom_nguoi_dung_2_dieu_huong = new NhomNguoiDung_2_DieuHuong();
                        nhom_nguoi_dung_2_dieu_huong.nhom_nguoi_dung_id = entity.id;
                        nhom_nguoi_dung_2_dieu_huong.dieu_huong_id = item.id;
                        entity.ds_dieu_huong.Add(nhom_nguoi_dung_2_dieu_huong);
                    }
                }
                #endregion
            }
            else
            {
                //var nguoiDung = _dbContext.quyen_nguoi_dung.Find(entity.id);
                entity.ds_dieu_huong.Clear();

                #region add điều hướng vào bảng trung gian
                if (nhomnguoidungDTO.ds_dieuhuong != null)
                {
                    foreach (var item in nhomnguoidungDTO.ds_dieuhuong)
                    {
                        NhomNguoiDung_2_DieuHuong nhom_nguoi_dung_2_dieu_huong = new NhomNguoiDung_2_DieuHuong();
                        nhom_nguoi_dung_2_dieu_huong.nhom_nguoi_dung_id = entity.id;
                        nhom_nguoi_dung_2_dieu_huong.dieu_huong_id = item.id;
                        entity.ds_dieu_huong.Add(nhom_nguoi_dung_2_dieu_huong);
                    }
                }
                #endregion
            }
        }
        public bool AddUserToGroup(UpdateNguoiDungFromNhomND obj)
        {
            try
            {
                var nhomNguoiDung = _dbContext.NhomNguoiDung.Find(obj.nhom_id);
                nhomNguoiDung.ds_nguoi_dung.Clear();
                if (obj != null)
                {
                    foreach (var item in obj.lts_nguoi_dung_id)
                    {
                        NguoiDung_2_NhomNguoiDung nguoi_dung_2_nhom_nguoi_dung = new NguoiDung_2_NhomNguoiDung();
                        nguoi_dung_2_nhom_nguoi_dung.nhom_nguoi_dung_id = obj.nhom_id;
                        nguoi_dung_2_nhom_nguoi_dung.nguoi_dung_id = item;
                        nhomNguoiDung.ds_nguoi_dung.Add(nguoi_dung_2_nhom_nguoi_dung);
                        _dbContext.NhomNguoiDung.Update(nhomNguoiDung);
                    }
                    if (_dbContext.SaveChanges() >= 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool AddPermission(AddPermission obj)
        {
            try
            {
                var nhomNguoiDung = _dbContext.NhomNguoiDung.Find(obj.nhom_id);
                nhomNguoiDung.ds_dieu_huong.Clear();
                if (obj != null)
                {
                    foreach (var item in obj.lts_dieu_huong_id)
                    {
                        NhomNguoiDung_2_DieuHuong nhom_nguoi_dung_2_dieu_huong = new NhomNguoiDung_2_DieuHuong();
                        nhom_nguoi_dung_2_dieu_huong.nhom_nguoi_dung_id = obj.nhom_id;
                        nhom_nguoi_dung_2_dieu_huong.dieu_huong_id = item;
                        nhomNguoiDung.ds_dieu_huong.Add(nhom_nguoi_dung_2_dieu_huong);
                        _dbContext.NhomNguoiDung.Update(nhomNguoiDung);
                    }
                    if (_dbContext.SaveChanges() >= 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
