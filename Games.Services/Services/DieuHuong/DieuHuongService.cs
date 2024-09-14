using AutoMapper;
using Games.Data.Models;
using Games.Services.Common;
using Games.Services.Models;
using Games.Services.Services.DieuHuong.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Services.Services.DieuHuong
{
    public class DieuHuongService : GenericService<Data.Models.QTHT.DieuHuong>, IDieuHuongService
    {
        IHttpContextAccessor _httpContextAccessor;
        CurrentUser cU;
        public DieuHuongService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
           : base(dbContext, httpContextAccessor)
        {
            ///Khởi tạo mapperconfiuration
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
                cfg.AddProfile<DieuHuongProfile>();
                //cfg.AddProfile<nguoi_dung>();
            });
            _mapper = _mapperCfg.CreateMapper();
            _mapperCfg.AssertConfigurationIsValid();
            _httpContextAccessor = httpContextAccessor;
            cU = new CurrentUser(_httpContextAccessor);
        }

        protected override IQueryable<Data.Models.QTHT.DieuHuong> QueryBuilder(IQueryable<Data.Models.QTHT.DieuHuong> query, dynamic filter, string search)
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
                String duong_dan = filter.duong_dan;
                if (!string.IsNullOrEmpty(duong_dan))
                {
                    duong_dan = duong_dan.ToLower().Trim();
                    query = query.Where(x => x.duong_dan.ToLower().Contains(duong_dan));
                }
                String mo_ta = filter.mo_ta;
                if (!string.IsNullOrEmpty(mo_ta))
                {
                    mo_ta = mo_ta.ToLower().Trim();
                    query = query.Where(x => x.mo_ta.ToLower().Contains(mo_ta));
                }
            }
            if (search != null && search != "")
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.ma.ToLower().Contains(search) ||
                                         x.ten.ToLower().Contains(search) ||
                                         x.duong_dan.ToLower().Contains(search) ||
                                         x.mo_ta.ToLower().Contains(search));
            }
            return query;
        }
        protected override void BeforeMapper<TDto>(bool isNew, ref TDto dto)
        {
            if (typeof(TDto) == typeof(DieuHuongDTO))
            {
                DieuHuongDTO dieuhuongDTO = (DieuHuongDTO)(object)dto;
                //xu ly tai day
                var checkExistMa = _repo.Where(x => x.ma.ToLower() == dieuhuongDTO.ma.ToLower() && x.id != dieuhuongDTO.id);
                if (checkExistMa.Count() > 0)
                {
                    ErrorCtr.DataExits("Mã điều hướng đã tồn tại, vui lòng nhập mã khác.");
                }

                var isExits = _repo.Where(x => x.ma.ToLower() == dieuhuongDTO.ma.ToLower() && x.ten.ToLower() == dieuhuongDTO.ten.ToLower() && x.dieu_huong_cap_tren_id == dieuhuongDTO.dieu_huong_cap_tren_id && x.id != dieuhuongDTO.id).Count() > 0;
                if (isExits)
                {
                    ErrorCtr.DataExits("Điều hướng đã tồn tại, vui lòng kiểm tra lại thông tin");
                }
                if (dieuhuongDTO.dieu_huong_cap_tren_id == dieuhuongDTO.id)
                {
                    ErrorCtr.DataExits("Điều hướng cấp trên không được trùng với điều hướng hiện tại.");
                }
                /// var stt_demo  = _repo.Where(x => x.so_thu_tu == dieuhuongDTO.so_thu_tu && x.dieu_huong_cap_tren_id == dieuhuongDTO.dieu_huong_cap_tren_id).ToList();
                if (isNew)
                {
                    var stt = _repo.Where(x => x.so_thu_tu == dieuhuongDTO.so_thu_tu && x.dieu_huong_cap_tren_id == dieuhuongDTO.dieu_huong_cap_tren_id).Count() > 0;
                    if (stt)
                    {
                        ErrorCtr.DataExits("Số thứ tự đã tồn tại, vui lòng kiểm tra lại thông tin");
                    }
                }
                else
                {
                    ///   var stt = _repo.Where(x => x.so_thu_tu == dieuhuongDTO.so_thu_tu && x.dieu_huong_cap_tren_id == dieuhuongDTO.dieu_huong_cap_tren_id)
                    var stt = _repo.Where(x => x.so_thu_tu == dieuhuongDTO.so_thu_tu && x.dieu_huong_cap_tren_id == dieuhuongDTO.dieu_huong_cap_tren_id && x.id != dieuhuongDTO.id).Count() >= 1;
                    if (stt)
                    {
                        ErrorCtr.DataExits("Số thứ tự đã tồn tại, vui lòng kiểm tra lại thông tin");
                    }
                }

                var itemtochuc = RecursiveItemDieuHuong(dieuhuongDTO.dieu_huong_cap_tren_id);
                if (isNew != true)
                {
                    var oldDieuhuong = _repo.Find(dieuhuongDTO.id);
                    if (oldDieuhuong.dieu_huong_cap_tren_id != dieuhuongDTO.dieu_huong_cap_tren_id || oldDieuhuong.ten != dieuhuongDTO.ten)
                    {
                        if (itemtochuc != null)
                        {
                            //dieuhuongDTO.ten_day_du = itemtochuc.ten_day_du + " - " + dieuhuongDTO.ten;
                            dieuhuongDTO.muc_luc = itemtochuc.muc_luc + @"\" + dieuhuongDTO.id.ToString();
                            dieuhuongDTO.cap_dieu_huong = itemtochuc.cap_dieu_huong + 1;
                        }
                        else
                        {
                            //dieuhuongDTO.ten_day_du = dieuhuongDTO.ten;
                            dieuhuongDTO.muc_luc = dieuhuongDTO.id.ToString();
                            dieuhuongDTO.cap_dieu_huong = 1;
                        }
                        //Kiem tra cap nhat lai tat ca muc luc con cua dia ban hien tai
                        var cuItem = new ItemDieuHuong
                        {
                            //ten_day_du = dieuhuongDTO.ten_day_du,
                            muc_luc = dieuhuongDTO.muc_luc,
                            cap_dieu_huong = dieuhuongDTO.cap_dieu_huong
                        };
                        var list = oldDieuhuong.ds_dieu_huong_cap_duoi.ToList();
                        if (list.Count() > 0)
                        {
                            foreach (var t in list)
                            {
                                RecursiveUpdateEntityDieuHuong(t, cuItem);
                            }
                        }
                    }
                }
            }
        }
        protected override void BeforeAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.QTHT.DieuHuong entity)
        {
            if (typeof(TDto) != typeof(DieuHuongDTO))
            {
                return;
            }
            DieuHuongDTO dieuhuongDTO = (DieuHuongDTO)(object)dto;
            if (dieuhuongDTO.dieu_huong_cap_tren_id == new Guid() || dieuhuongDTO.dieu_huong_cap_tren_id == null)
            {
                //entity.cap_dieu_huong = 1;
            }
            else
            {
                var cap = _repo.Where(x => x.id == dieuhuongDTO.dieu_huong_cap_tren_id)?.FirstOrDefault().cap_dieu_huong;
                if (cap != null && cap > 0)
                {
                    entity.cap_dieu_huong = Convert.ToInt32(cap) + 1;
                }
            }
            #region update số thứ tự order

            var stt_order_cha = _dbContext.DieuHuong.Where(x => x.id == dieuhuongDTO.dieu_huong_cap_tren_id).Select(y => y.stt_order).FirstOrDefault();
            entity.stt_order = stt_order_cha + ((stt_order_cha == "" || stt_order_cha == null) ? "" : ".") + Stt(dieuhuongDTO.so_thu_tu.Value);

            if (!isNew)
            {
                var id_dh = entity.id;
                var muc_luc_record_current = _repo.Find(dieuhuongDTO.id).muc_luc;
                var list = _dbContext.DieuHuong.Where(x => x.muc_luc.StartsWith(muc_luc_record_current) && x.id != id_dh);
                if (list != null && list.Any())
                {
                    var lth = entity.stt_order.Length;
                    foreach (var item in list)
                    {
                        var suffix = item.stt_order.Substring(item.stt_order.Length - (3 * (item.cap_dieu_huong - entity.cap_dieu_huong) + 1));
                        //var suffix = item.stt_order.Substring(lth);
                        item.stt_order = entity.stt_order + suffix;

                        _repo.Update(item);
                    }
                }
            }
            #endregion
        }

        protected override void AfferAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.QTHT.DieuHuong entity)
        {
            if (typeof(TDto) == typeof(DieuHuongDTO))
            {
                DieuHuongDTO dieuhuongDto = (DieuHuongDTO)(object)dto;
                var itemDieuHuong = RecursiveItemDieuHuong(dieuhuongDto.dieu_huong_cap_tren_id);
                //if (isNew == true)
                //{
                if (itemDieuHuong != null)
                {
                    //entity.ten_day_du = itemDieuHuong.ten_day_du + " - " + entity.ten;
                    entity.muc_luc = itemDieuHuong.muc_luc + @"\" + entity.id.ToString();
                    entity.cap_dieu_huong = itemDieuHuong.cap_dieu_huong + 1;
                }
                else
                {
                    //entity.ten_day_du = entity.ten;
                    entity.muc_luc = entity.id.ToString();
                    entity.cap_dieu_huong = 1;
                }

                //}
            }

        }

        public List<TreeDieuHuongDTO> GetTreeDieuHuong()
        {
            try
            {
                List<TreeDieuHuongDTO> tree = new List<TreeDieuHuongDTO>();
                var currentUser = cU.Get();
                if (!currentUser.super_admin)
                {
                    var mapperCfg = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BaseProfile>();
                        cfg.CreateMap<Data.Models.QTHT.DieuHuong, TreeDieuHuongDTO>()
                        .ForMember(x => x.ma, otp => otp.MapFrom(z => z.ma))
                        .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                        .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.url, otp => otp.MapFrom(z => z.duong_dan))
                        .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.icon, otp => otp.MapFrom(z => z.icon))
                        .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_dieu_huong_cap_duoi.Where(x => x.is_router == true && x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == currentUser.id))).OrderBy(x => x.so_thu_tu)));
                    });
                    var mapper = mapperCfg.CreateMapper();
                    mapperCfg.AssertConfigurationIsValid();

                    var ltsDieuHuong = _repo.Where(x => x.cap_dieu_huong == 1 && x.is_router == true && x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == currentUser.id))).OrderBy(x => x.so_thu_tu).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        tree = mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<TreeDieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                else
                {
                    var mapperCfg = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BaseProfile>();
                        cfg.CreateMap<Data.Models.QTHT.DieuHuong, TreeDieuHuongDTO>()
                        .ForMember(x => x.ma, otp => otp.MapFrom(z => z.ma))
                        .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                        .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.url, otp => otp.MapFrom(z => z.duong_dan))
                        .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.icon, otp => otp.MapFrom(z => z.icon))
                        .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_dieu_huong_cap_duoi.Where(t => t.is_router == true).OrderBy(x => x.so_thu_tu)));
                    });
                    var mapper = mapperCfg.CreateMapper();
                    mapperCfg.AssertConfigurationIsValid();
                    var ltsDieuHuong = _repo.Where(x => x.cap_dieu_huong == 1 && x.is_router == true).OrderBy(x => x.so_thu_tu).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        tree = mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<TreeDieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                return tree;
                //return tree.Where(x => x.stt_order.Length == 3).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeDieuHuongDTO> GetTreeDieuHuongForm()
        {
            try
            {
                List<TreeDieuHuongDTO> tree = new List<TreeDieuHuongDTO>();
                var currentUser = cU.Get();
                if (!currentUser.super_admin)
                {
                    var mapperCfg = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BaseProfile>();
                        cfg.CreateMap<Data.Models.QTHT.DieuHuong, TreeDieuHuongDTO>()
                        .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                        .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.url, otp => otp.MapFrom(z => z.duong_dan))
                        .ForMember(x => x.so_thu_tu, otp => otp.MapFrom(z => z.so_thu_tu))
                        .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.icon, otp => otp.MapFrom(z => z.icon))
                        .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_dieu_huong_cap_duoi.Where(x => x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == currentUser.id))).OrderBy(x => x.so_thu_tu)));
                    });
                    var mapper = mapperCfg.CreateMapper();
                    mapperCfg.AssertConfigurationIsValid();

                    var ltsDieuHuong = _repo.Where(x => x.cap_dieu_huong == 1 && x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == currentUser.id))).OrderBy(x => x.so_thu_tu).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        tree = mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<TreeDieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                else
                {
                    var mapperCfg = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BaseProfile>();
                        cfg.CreateMap<Data.Models.QTHT.DieuHuong, TreeDieuHuongDTO>()
                        .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                        .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.url, otp => otp.MapFrom(z => z.duong_dan))
                        .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                        .ForMember(x => x.icon, otp => otp.MapFrom(z => z.icon))
                        .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_dieu_huong_cap_duoi.OrderBy(x => x.so_thu_tu)));
                    });
                    var mapper = mapperCfg.CreateMapper();
                    mapperCfg.AssertConfigurationIsValid();
                    var ltsDieuHuong = _repo.Where(x => x.cap_dieu_huong == 1).OrderBy(x => x.so_thu_tu).ToList();

                    if (ltsDieuHuong.Any())
                    {
                        tree = mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<TreeDieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                return tree;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteByMucLuc(Guid id)
        {
            try
            {
                var muc_luc = _repo.Where(x => x.id == id).FirstOrDefault().muc_luc;
                var lts = _repo.Where(x => x.muc_luc.StartsWith(muc_luc));
                _repo.RemoveRange(lts);
                if (_dbContext.SaveChanges() >= 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? GetStt(Guid? dieu_huong_cap_tren_id)
        {
            try
            {
                var max = _dbContext.DieuHuong.Where(x => x.dieu_huong_cap_tren_id == dieu_huong_cap_tren_id).Select(y => y.so_thu_tu).Max() + 1;
                if (max != null)
                {
                    return max;
                }
                else
                {
                    return max = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ItemDieuHuong RecursiveItemDieuHuong(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            ItemDieuHuong item = new ItemDieuHuong();
            var cap_dieu_huong = 1;
            Stack<string> stackTenDayDu = new Stack<string>();
            Stack<string> stackMucLuc = new Stack<string>();
            var dieuhuong = _repo.Where(x => x.id == id.Value).FirstOrDefault();
            stackTenDayDu.Push(dieuhuong.ten);
            stackMucLuc.Push(dieuhuong.id.ToString());
            if (dieuhuong.dieu_huong_cap_tren_id != null)
            {
                var temp = RecursiveItemDieuHuong(dieuhuong.dieu_huong_cap_tren_id.Value);
                stackTenDayDu.Push(temp.ten_day_du);
                stackMucLuc.Push(temp.muc_luc);
                cap_dieu_huong = temp.cap_dieu_huong;
                cap_dieu_huong++;
            }
            item.ten_day_du = string.Join(" - ", stackTenDayDu);
            item.muc_luc = string.Join(@"\", stackMucLuc);
            item.cap_dieu_huong = cap_dieu_huong;
            return item;
        }

        #region recursive update muc luc, ten điều hướng cac cap con
        public List<Data.Models.QTHT.DieuHuong> RecursiveUpdateEntityDieuHuong(Data.Models.QTHT.DieuHuong entity, ItemDieuHuong parent)
        {

            entity.muc_luc = parent.muc_luc + @"\" + entity.id.ToString();
            entity.cap_dieu_huong = parent.cap_dieu_huong + 1;
            _repo.Update(entity);
            // tim tat cac dia ban co cap tren id = entity.id
            var list = entity.ds_dieu_huong_cap_duoi.ToList();
            var cuItem = new ItemDieuHuong
            {
                muc_luc = entity.muc_luc,
                cap_dieu_huong = entity.cap_dieu_huong
            };
            if (list.Count() > 0)
            {
                foreach (var t in list)
                {
                    RecursiveUpdateEntityDieuHuong(t, cuItem);
                }
            }
            return list;
        }
        #endregion

        public List<DieuHuongDTO> GetFlatDieuHuong()
        {
            try
            {
                List<DieuHuongDTO> res = new List<DieuHuongDTO>();
                var currentUser = cU.Get();
                if (!currentUser.super_admin)
                {
                    var ltsDieuHuong = _repo.Where(x => x.is_router == true && x.ds_nhom_nguoi_dung.Any(y => y.nhom_nguoi_dung.ds_nguoi_dung.Any(t => t.nguoi_dung_id == currentUser.id))).OrderBy(x => x.stt_order).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        res = _mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<DieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                else
                {
                    var ltsDieuHuong = _repo.Where(x => x.is_router == true).OrderBy(x => x.stt_order).ToList();
                    if (ltsDieuHuong.Any())
                    {
                        res = _mapper.Map<List<Data.Models.QTHT.DieuHuong>, List<DieuHuongDTO>>(ltsDieuHuong);
                    }
                }
                foreach (var item in res)
                {
                    if (item.cap_dieu_huong == 1)
                        item.is_show = true;
                    else
                        item.is_show = false;
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////hàm viết api update Sttorder
        public void UpdateSttOrder()
        {
            var lst_dieu_huong = GetTreeDieuHuongForm();
            SttOrder("", lst_dieu_huong);


        }
        ////hàm viết SttOrder
        public void SttOrder(string stt_order_cha, List<TreeDieuHuongDTO> treeDieuHuongs)
        {
            try
            {
                foreach (var item in treeDieuHuongs)
                {
                    item.stt_order = stt_order_cha + (stt_order_cha == "" ? "" : ".") + Stt(item.so_thu_tu.Value);
                    var dh = _dbContext.DieuHuong.Find(item.value);
                    dh.stt_order = item.stt_order;
                    _dbContext.DieuHuong.Update(dh);

                    if (item.children.Any())
                    {
                        SttOrder(item.stt_order, item.children);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////hàm update stt them dang "001.001"
        public string Stt(int? so_thu_tu)
        {
            var stt = so_thu_tu.ToString();
            if (stt.Length == 1)
            {
                stt = "00" + stt;
            }
            else if (stt.Length == 2)
            {
                stt = "0" + stt;
            }

            return stt;
        }
    }
}
