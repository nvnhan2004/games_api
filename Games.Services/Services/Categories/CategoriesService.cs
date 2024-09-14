using AutoMapper;
using Games.Data.Models;
using Games.Services.Common;
using Games.Services.Models;
using Games.Services.Services.Categories;
using Games.Services.Services.Categories.DTO;
using Games.Services.Services.Categories.DTO;
using Games.Services.Services.Games.DTO;
using Games.Services.Services.MailKit.DTO;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Services.Services.Categories
{
    public class CategoriesService : GenericService<Data.Models.ChucNang.Categories>, ICategoriesService
    {
        private readonly CategoriesConfiguarationDTO _categoriesConfiguaration;
        IHttpContextAccessor _httpContextAccessor;
        CurrentUser cU;
        public CategoriesService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, CategoriesConfiguarationDTO categoriesConfiguarationDTO)
           : base(dbContext, httpContextAccessor)
        {
            ///Khởi tạo mapperconfiuration
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
                cfg.AddProfile<CategoriesProfile>();
                //cfg.AddProfile<nguoi_dung>();
            });
            _mapper = _mapperCfg.CreateMapper();
            _mapperCfg.AssertConfigurationIsValid();
            _httpContextAccessor = httpContextAccessor;
            _categoriesConfiguaration = categoriesConfiguarationDTO;
            cU = new CurrentUser(_httpContextAccessor);
        }

        protected override IQueryable<Data.Models.ChucNang.Categories> QueryBuilder(IQueryable<Data.Models.ChucNang.Categories> query, dynamic filter, string search)
        {
            if (filter != null)
            {
                String ten = filter.ten;
                if (!string.IsNullOrEmpty(ten))
                {
                    ten = ten.ToLower().Trim();
                    query = query.Where(x => x.ten.ToLower().Contains(ten));
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
                query = query.Where(x => x.ten.ToLower().Contains(search) ||
                                         x.mo_ta.ToLower().Contains(search));
            }
            return query;
        }
        protected override void BeforeMapper<TDto>(bool isNew, ref TDto dto)
        {
            if (typeof(TDto) == typeof(CategoriesDTO))
            {
                CategoriesDTO categoriesDTO = (CategoriesDTO)(object)dto;
                //xu ly tai day

                var isExits = _repo.Where(x => x.ten.ToLower() == categoriesDTO.ten.ToLower() && x.categories_cap_tren_id == categoriesDTO.categories_cap_tren_id && x.id != categoriesDTO.id).Count() > 0;
                if (isExits)
                {
                    ErrorCtr.DataExits("Danh mục đã tồn tại, vui lòng kiểm tra lại thông tin");
                }
                if (categoriesDTO.categories_cap_tren_id == categoriesDTO.id)
                {
                    ErrorCtr.DataExits("Danh mục cấp trên không được trùng với danh mục hiện tại.");
                }
                /// var stt_demo  = _repo.Where(x => x.so_thu_tu == categoriesDTO.so_thu_tu && x.categories_cap_tren_id == categoriesDTO.categories_cap_tren_id).ToList();
                if (isNew)
                {
                    var stt = _repo.Where(x => x.so_thu_tu == categoriesDTO.so_thu_tu && x.categories_cap_tren_id == categoriesDTO.categories_cap_tren_id).Count() > 0;
                    if (stt)
                    {
                        ErrorCtr.DataExits("Số thứ tự đã tồn tại, vui lòng kiểm tra lại thông tin");
                    }
                }
                else
                {
                    ///   var stt = _repo.Where(x => x.so_thu_tu == categoriesDTO.so_thu_tu && x.categories_cap_tren_id == categoriesDTO.categories_cap_tren_id)
                    var stt = _repo.Where(x => x.so_thu_tu == categoriesDTO.so_thu_tu && x.categories_cap_tren_id == categoriesDTO.categories_cap_tren_id && x.id != categoriesDTO.id).Count() >= 1;
                    if (stt)
                    {
                        ErrorCtr.DataExits("Số thứ tự đã tồn tại, vui lòng kiểm tra lại thông tin");
                    }
                }

                var itemtochuc = RecursiveItemCategories(categoriesDTO.categories_cap_tren_id);
                if (isNew != true)
                {
                    var oldDieuhuong = _repo.Find(categoriesDTO.id);
                    if (oldDieuhuong.categories_cap_tren_id != categoriesDTO.categories_cap_tren_id || oldDieuhuong.ten != categoriesDTO.ten)
                    {
                        if (itemtochuc != null)
                        {
                            //categoriesDTO.ten_day_du = itemtochuc.ten_day_du + " - " + categoriesDTO.ten;
                            categoriesDTO.muc_luc = itemtochuc.muc_luc + @"\" + categoriesDTO.id.ToString();
                            categoriesDTO.cap = itemtochuc.cap + 1;
                        }
                        else
                        {
                            //categoriesDTO.ten_day_du = categoriesDTO.ten;
                            categoriesDTO.muc_luc = categoriesDTO.id.ToString();
                            categoriesDTO.cap = 1;
                        }
                        //Kiem tra cap nhat lai tat ca muc luc con cua dia ban hien tai
                        var cuItem = new ItemCategories
                        {
                            //ten_day_du = categoriesDTO.ten_day_du,
                            muc_luc = categoriesDTO.muc_luc,
                            cap = categoriesDTO.cap
                        };
                        var list = oldDieuhuong.ds_categories_cap_duoi.ToList();
                        if (list.Count() > 0)
                        {
                            foreach (var t in list)
                            {
                                RecursiveUpdateEntityCategories(t, cuItem);
                            }
                        }
                    }
                }
            }
        }
        protected override void BeforeAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.ChucNang.Categories entity)
        {
            if (typeof(TDto) != typeof(CategoriesDTO))
            {
                return;
            }
            CategoriesDTO categoriesDTO = (CategoriesDTO)(object)dto;
            if (categoriesDTO.categories_cap_tren_id == new Guid() || categoriesDTO.categories_cap_tren_id == null)
            {
                //entity.cap_dieu_huong = 1;
            }
            else
            {
                var cap = _repo.Where(x => x.id == categoriesDTO.categories_cap_tren_id)?.FirstOrDefault().cap;
                if (cap != null && cap > 0)
                {
                    entity.cap = Convert.ToInt32(cap) + 1;
                }
            }
            #region update số thứ tự order

            var stt_order_cha = _dbContext.Categories.Where(x => x.id == categoriesDTO.categories_cap_tren_id).Select(y => y.stt_order).FirstOrDefault();
            entity.stt_order = stt_order_cha + ((stt_order_cha == "" || stt_order_cha == null) ? "" : ".") + Stt(categoriesDTO.so_thu_tu.Value);

            if (!isNew)
            {
                var id_dh = entity.id;
                var muc_luc_record_current = _repo.Find(categoriesDTO.id).muc_luc;
                var list = _dbContext.Categories.Where(x => x.muc_luc.StartsWith(muc_luc_record_current) && x.id != id_dh);
                if (list != null && list.Any())
                {
                    var lth = entity.stt_order.Length;
                    foreach (var item in list)
                    {
                        var suffix = item.stt_order.Substring(item.stt_order.Length - (3 * (item.cap - entity.cap) + 1));
                        //var suffix = item.stt_order.Substring(lth);
                        item.stt_order = entity.stt_order + suffix;

                        _repo.Update(item);
                    }
                }
            }
            #endregion
        }

        protected override void AfferAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.ChucNang.Categories entity)
        {
            if (typeof(TDto) == typeof(CategoriesDTO))
            {
                CategoriesDTO dieuhuongDto = (CategoriesDTO)(object)dto;
                var itemCategories = RecursiveItemCategories(dieuhuongDto.categories_cap_tren_id);
                //if (isNew == true)
                //{
                if (itemCategories != null)
                {
                    //entity.ten_day_du = itemCategories.ten_day_du + " - " + entity.ten;
                    entity.muc_luc = itemCategories.muc_luc + @"\" + entity.id.ToString();
                    entity.cap = itemCategories.cap + 1;
                }
                else
                {
                    //entity.ten_day_du = entity.ten;
                    entity.muc_luc = entity.id.ToString();
                    entity.cap = 1;
                }

                //}

                if (isNew == false)
                {
                    if(entity.su_dung == false)
                    {
                        if (entity.ds_categories_cap_duoi != null && entity.ds_categories_cap_duoi.Any())
                        {
                            foreach (var item in entity.ds_categories_cap_duoi)
                            {
                                item.su_dung = false;
                            }
                        }
                    }
                }
            }

        }

        public List<TreeCategoriesDTO> GetTreeCategories()
        {
            try
            {
                List<TreeCategoriesDTO> tree = new List<TreeCategoriesDTO>();
                
                var mapperCfg = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BaseProfile>();
                    cfg.CreateMap<Data.Models.ChucNang.Categories, TreeCategoriesDTO>()
                    .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                    .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                    .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                    .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_categories_cap_duoi.OrderBy(x => x.so_thu_tu)));
                });
                var mapper = mapperCfg.CreateMapper();
                mapperCfg.AssertConfigurationIsValid();
                var ltsCategories = _repo.Where(x => x.cap == 1).OrderBy(x => x.so_thu_tu).ToList();
                if (ltsCategories.Any())
                {
                    tree = mapper.Map<List<Data.Models.ChucNang.Categories>, List<TreeCategoriesDTO>>(ltsCategories);
                }
                
                return tree;
                //return tree.Where(x => x.stt_order.Length == 3).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeCategoriesDTO> GetTreeCategoriesForm()
        {
            try
            {
                List<TreeCategoriesDTO> tree = new List<TreeCategoriesDTO>();
                
                var mapperCfg = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BaseProfile>();
                    cfg.CreateMap<Data.Models.ChucNang.Categories, TreeCategoriesDTO>()
                    .ForMember(x => x.value, otp => otp.MapFrom(z => z.id))
                    .ForMember(x => x.label, otp => otp.MapFrom(z => z.ten))
                    .ForMember(x => x.name, otp => otp.MapFrom(z => z.ten))
                    .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_categories_cap_duoi.OrderBy(x => x.so_thu_tu)));
                });
                var mapper = mapperCfg.CreateMapper();
                mapperCfg.AssertConfigurationIsValid();
                var ltsCategories = _repo.Where(x => x.cap == 1).OrderBy(x => x.so_thu_tu).ToList();

                if (ltsCategories.Any())
                {
                    tree = mapper.Map<List<Data.Models.ChucNang.Categories>, List<TreeCategoriesDTO>>(ltsCategories);
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

        public int? GetStt(Guid? categories_cap_tren_id)
        {
            try
            {
                var max = _dbContext.Categories.Where(x => x.categories_cap_tren_id == categories_cap_tren_id).Select(y => y.so_thu_tu).Max() + 1;
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

        public ItemCategories RecursiveItemCategories(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            ItemCategories item = new ItemCategories();
            var cap_dieu_huong = 1;
            Stack<string> stackTenDayDu = new Stack<string>();
            Stack<string> stackMucLuc = new Stack<string>();
            var dieuhuong = _repo.Where(x => x.id == id.Value).FirstOrDefault();
            stackTenDayDu.Push(dieuhuong.ten);
            stackMucLuc.Push(dieuhuong.id.ToString());
            if (dieuhuong.categories_cap_tren_id != null)
            {
                var temp = RecursiveItemCategories(dieuhuong.categories_cap_tren_id.Value);
                stackTenDayDu.Push(temp.ten_day_du);
                stackMucLuc.Push(temp.muc_luc);
                cap_dieu_huong = temp.cap;
                cap_dieu_huong++;
            }
            item.ten_day_du = string.Join(" - ", stackTenDayDu);
            item.muc_luc = string.Join(@"\", stackMucLuc);
            item.cap = cap_dieu_huong;
            return item;
        }

        #region recursive update muc luc, ten điều hướng cac cap con
        public List<Data.Models.ChucNang.Categories> RecursiveUpdateEntityCategories(Data.Models.ChucNang.Categories entity, ItemCategories parent)
        {

            entity.muc_luc = parent.muc_luc + @"\" + entity.id.ToString();
            entity.cap = parent.cap + 1;
            _repo.Update(entity);
            // tim tat cac dia ban co cap tren id = entity.id
            var list = entity.ds_categories_cap_duoi.ToList();
            var cuItem = new ItemCategories
            {
                muc_luc = entity.muc_luc,
                cap = entity.cap
            };
            if (list.Count() > 0)
            {
                foreach (var t in list)
                {
                    RecursiveUpdateEntityCategories(t, cuItem);
                }
            }
            return list;
        }
        #endregion

        public List<CategoriesDTO> GetFlatCategories()
        {
            try
            {
                List<CategoriesDTO> res = new List<CategoriesDTO>();
                var currentUser = cU.Get();
                
                var ltsCategories = _repo.OrderBy(x => x.stt_order).ToList();
                if (ltsCategories.Any())
                {
                    res = _mapper.Map<List<Data.Models.ChucNang.Categories>, List<CategoriesDTO>>(ltsCategories);
                }
                
                foreach (var item in res)
                {
                    if (item.cap == 1)
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
            var lst_dieu_huong = GetTreeCategoriesForm();
            SttOrder("", lst_dieu_huong);


        }
        ////hàm viết SttOrder
        public void SttOrder(string stt_order_cha, List<TreeCategoriesDTO> treeCategoriess)
        {
            try
            {
                foreach (var item in treeCategoriess)
                {
                    item.stt_order = stt_order_cha + (stt_order_cha == "" ? "" : ".") + Stt(item.so_thu_tu.Value);
                    var dh = _dbContext.Categories.Find(item.value);
                    dh.stt_order = item.stt_order;
                    _dbContext.Categories.Update(dh);

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

        public string CrawlCategories()
        {
            try
            {
                var res = "";
                int stt = 1;
                var cateFromDb = _repo.Select(x => new
                {
                    id = x.id,
                    ten = x.ten,
                    cap = x.cap,
                    url_crawl = x.url_crawl,
                    stt_order = x.stt_order,
                    categories_cap_tren_ten = x.categories_cap_tren != null ? x.categories_cap_tren.ten : ""
                }).ToList();

                

                var web = new HtmlWeb();
                var htmlCategoriesDoc = web.Load(_categoriesConfiguaration.url_crawl);
                var textNodesCate = htmlCategoriesDoc.DocumentNode.SelectNodes("//div[@class='all_label_list-wrapper']/div[@class='label-wrapper']");
                if (textNodesCate != null)
                {
                    foreach (var node in textNodesCate)
                    {
                        var ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//h2[contains(@class,'label-title box-title ellipsis')]/a").InnerText);
                        var url_crawl = node.SelectSingleNode(".//h2[contains(@class,'label-title box-title ellipsis')]/a").Attributes["href"].Value;
                        if (cateFromDb.Where(x => x.ten == ten && x.cap == 1).Count() == 0)
                        {
                            Guid cate_id = Guid.NewGuid();

                            Data.Models.ChucNang.Categories cate = new Data.Models.ChucNang.Categories();

                            cate.id = cate_id;
                            cate.ten = ten;
                            cate.cap = 1;
                            cate.ngay_tao = DateTime.Now;
                            cate.ngay_chinh_sua = DateTime.Now;
                            cate.categories_cap_tren_id = null;
                            cate.img = null;
                            cate.mo_ta = null;
                            cate.is_crawl = false;
                            cate.muc_luc = cate_id.ToString();
                            cate.so_thu_tu = stt;
                            cate.stt_order = Stt(stt);
                            cate.su_dung = true;
                            cate.url_crawl = url_crawl;
                            cate.total_game = 0;
                            cate.total_error_game = 0;
                            cate.slug = AppConst.GetSlug(ten);

                            _repo.Add(cate);

                            var textNodesChildCate = node.SelectNodes(".//ul[@class='label_list']//li");
                            if (textNodesChildCate != null)
                            {
                                int stt_child = 1;
                                foreach (var child in textNodesChildCate)
                                {
                                    var ten_child = HtmlEntity.DeEntitize(child.SelectSingleNode(".//a[contains(@class,'label_list-item-link')]").InnerText);
                                    var url_crawl_child = child.SelectSingleNode(".//a[contains(@class,'label_list-item-link')]").Attributes["href"].Value;

                                    if (cateFromDb.Where(x => x.ten == ten_child && x.cap == 2 && x.categories_cap_tren_ten == ten).Count() == 0)
                                    {
                                        Guid cate_child_id = Guid.NewGuid();
                                        var stt_order_cha = cate.stt_order;
                                        var stt_order = stt_order_cha + "." + Stt(stt_child);

                                        Data.Models.ChucNang.Categories cate_child = new Data.Models.ChucNang.Categories();

                                        cate_child.id = cate_child_id;
                                        cate_child.ten = ten_child;
                                        cate_child.cap = 2;
                                        cate_child.ngay_tao = DateTime.Now;
                                        cate_child.ngay_chinh_sua = DateTime.Now;
                                        cate_child.categories_cap_tren_id = cate_id;
                                        cate_child.img = null;
                                        cate_child.mo_ta = null;
                                        cate_child.is_crawl = false;
                                        cate_child.muc_luc = cate_id.ToString() + @"\" + cate_child_id.ToString();
                                        cate_child.so_thu_tu = stt_child;
                                        cate_child.stt_order = stt_order;
                                        cate_child.su_dung = true;
                                        cate_child.url_crawl = url_crawl_child;
                                        cate_child.total_game = 0;
                                        cate_child.total_error_game = 0;
                                        cate_child.slug = AppConst.GetSlug(ten_child);

                                        _repo.Add(cate_child);
                                    }
                                    stt_child++;
                                }
                            }
                        }
                        stt++;
                    }

                    int countCate = _dbContext.SaveChanges();
                    if (countCate > 0)
                    {
                        return $"Thêm mới thành công {countCate} categories";
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeCategoriesPublicDTO> GetCategoriesMenu()
        {
            try
            {
                List<TreeCategoriesPublicDTO> tree = new List<TreeCategoriesPublicDTO>();

                var mapperCfg = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BaseProfile>();
                    cfg.CreateMap<Data.Models.ChucNang.Categories, TreeCategoriesPublicDTO>()
                    .ForMember(x => x.ds_games, opt => opt.Ignore())
                    .ForMember(x => x.children, otp => otp.MapFrom(z => z.ds_categories_cap_duoi.Where(x => x.su_dung == true && x.is_menu == true).Take(8).OrderBy(t => t.so_thu_tu)));
                });
                var mapper = mapperCfg.CreateMapper();
                mapperCfg.AssertConfigurationIsValid();
                var ltsCategories = _repo.Where(x => x.cap == 1 && x.is_menu == true).OrderBy(x => x.so_thu_tu).ToList();

                if (ltsCategories.Any())
                {
                    tree = mapper.Map<List<Data.Models.ChucNang.Categories>, List<TreeCategoriesPublicDTO>>(ltsCategories);
                }

                return tree;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TreeCategoriesPublicDTO GetDetailCategories(string slug)
        {
            try
            {
                var res = new TreeCategoriesPublicDTO();

                var mapperCfg = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BaseProfile>();
                    cfg.CreateMap<Data.Models.ChucNang.Categories, TreeCategoriesPublicDTO>()
                    .ForMember(x => x.children, opt => opt.Ignore())
                    .ForMember(x => x.ds_games, otp => otp.MapFrom(z => z.ds_games.Where(x => x.su_dung == true).Select(t => new GamesPublicDTO
                    {
                        id = t.id,
                        description= t.description,
                        img = t.img,
                        is_menu = t.is_menu,
                        is_new = t.is_new,
                        is_trending = t.is_trending,
                        slug= t.slug,
                        ten = t.ten,
                        title = t.title
                    }).Take(50).ToList()));
                });
                var mapper = mapperCfg.CreateMapper();
                mapperCfg.AssertConfigurationIsValid();
                var categories = _repo.Where(x => x.slug == slug).FirstOrDefault();

                if (categories != null)
                {
                    res = mapper.Map<Data.Models.ChucNang.Categories, TreeCategoriesPublicDTO>(categories);
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
