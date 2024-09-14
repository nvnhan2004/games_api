using AutoMapper;
using Games.Data.Models;
using Games.Data.Models.ChucNang;
using Games.Services.Common;
using Games.Services.Models;
using Games.Services.Services.Categories.DTO;
using Games.Services.Services.Games.DTO;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Games.Services.Services.Games
{
    public class GamesService : GenericService<Data.Models.ChucNang.Games>, IGamesService
    {
        IHttpContextAccessor _httpContextAccessor;
        public GamesService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
           : base(dbContext, httpContextAccessor)
        {
            ///Khởi tạo mapperconfiuration
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
                cfg.AddProfile<GamesProfile>();
            });
            _mapper = _mapperCfg.CreateMapper();
            _mapperCfg.AssertConfigurationIsValid();
            _httpContextAccessor = httpContextAccessor;
        }

        protected override IQueryable<Data.Models.ChucNang.Games> QueryBuilder(IQueryable<Data.Models.ChucNang.Games> query, dynamic filter, string search)
        {
            if (filter != null)
            {
                String ten = filter.ten;
                if (!string.IsNullOrEmpty(ten))
                {
                    ten = ten.ToLower().Trim();
                    query = query.Where(x => x.ten.ToLower().Contains(ten));
                }
                String so_thu_tu = filter.so_thu_tu;
                if (!string.IsNullOrEmpty(so_thu_tu))
                {
                    int stt = Int16.Parse(so_thu_tu);
                    query = query.Where(x => x.so_thu_tu == stt);
                }
                String is_new = filter.is_new;
                if (!string.IsNullOrEmpty(is_new))
                {
                    bool isNew = Boolean.Parse(is_new);
                    query = query.Where(x => x.is_new == isNew);
                }
                String is_trending = filter.is_trending;
                if (!string.IsNullOrEmpty(is_trending))
                {
                    bool isTrending = Boolean.Parse(is_trending);
                    query = query.Where(x => x.is_trending == isTrending);
                }
                String category_id = filter.category_id;
                if (!string.IsNullOrEmpty(category_id))
                {
                    Guid cate_id = Guid.Parse(category_id);
                    query = query.Where(x => x.category_id == cate_id);
                }
            }
            if (search != null && search != "")
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.ten.ToLower().Contains(search));
            }
            return query;
        }
        protected override void BeforeMapper<TDto>(bool isNew, ref TDto dto)
        {
            if (typeof(TDto) == typeof(GamesDTO))
            {
                GamesDTO gamesDTO = (GamesDTO)(object)dto;
                var isExits = _repo.Where(x => x.ten.ToLower() == gamesDTO.ten.ToLower() && x.category_id == gamesDTO.category_id && x.id != gamesDTO.id);
                if (isExits.Count() > 0)
                {
                    ErrorCtr.DataExits("Teen games đã tồn tại, vui lòng kiểm tra lại thông tin");
                }
            }
        }
        protected override void BeforeAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.ChucNang.Games entity)
        {
            if (typeof(TDto) != typeof(GamesDTO))
            {
                return;
            }
            GamesDTO nguoidungDTO = (GamesDTO)(object)dto;
            if (isNew)
            {

            }
        }
        protected override void AfferAddOrUpdate<TDto>(bool isNew, ref TDto dto, ref Data.Models.ChucNang.Games entity)
        {
            if (typeof(TDto) != typeof(GamesDTO))
            {
                return;
            }
        }

        public async Task<string> CrawlGames(RequestCrawlGamesDTO requestCrawl)
        {
            try
            {
                var cateUpdateTotal = _dbContext.Categories.Find(requestCrawl.category_id);
                var ltsCate = _dbContext.Categories.Select(x => new
                {
                    id = x.id,
                    ten = x.ten,
                    url_crawl = x.url_crawl,
                    img = x.img,
                    cap = x.cap,
                }).ToList();
                var cate = ltsCate.Where(x => x.id == requestCrawl.category_id).FirstOrDefault();
                if(cate == null || cate.url_crawl == null || cate.url_crawl == "")
                {
                    return "Không có url crawl";
                }
                var gamesFromDb = _repo.Select(x => new
                {
                    ten = x.ten,
                    category_id = x.category_id
                }).ToList();

                var res = "Có lỗi trong quá trình crawl games";
                int total_game = 0;
                int total_error_game = 0;
                var web = new HtmlWeb();
                var htmlCategoriesDoc = web.Load(cate.url_crawl);

                #region update img của categories
                if (cateUpdateTotal.is_crawl != true)
                {
                    var relatedCategoriesNode = htmlCategoriesDoc.DocumentNode.SelectNodes(".//ul[@class='related_labels__menu']/li");
                    if (relatedCategoriesNode != null)
                    {
                        foreach (var node in relatedCategoriesNode)
                        {
                            var ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//p[contains(@class,'related_labels__menu__item-link__label__text')]").InnerText);
                            var img = node.SelectSingleNode(".//img[contains(@class,'tablet')]").Attributes["data-src"].Value;

                            var cateUpdateImg = ltsCate.Where(x => x.cap == 2 && x.ten == ten).FirstOrDefault();
                            if (cateUpdateImg != null)
                            {
                                var cateEntity = _dbContext.Categories.Find(cateUpdateImg.id);
                                cateEntity.img = img;
                                cateEntity.ngay_chinh_sua = DateTime.Now;
                                _dbContext.Categories.Update(cateEntity);
                            }
                        }
                    }
                }
                #endregion

                #region ckeck pagination 
                var stt = gamesFromDb.Count() + 1;
                var totalPage = 0;
                var pageLinks = htmlCategoriesDoc.DocumentNode.SelectNodes("//div[@class='button-group']//a");
                if (pageLinks != null)
                {
                    // Lọc những thẻ <a> có thuộc tính data-event-label (số trang) và chuyển thành số nguyên
                    totalPage = pageLinks
                        .Where(node => node.Attributes["data-event-label"] != null)
                        .Select(node => int.Parse(node.Attributes["data-event-label"].Value))
                        .Max();
                }
                #endregion

                var textNodesCate = htmlCategoriesDoc.DocumentNode.SelectNodes("//ul[@class='grid-row']/li");
                if (textNodesCate != null)
                {
                    List<Task<Data.Models.ChucNang.Games>> tasks = new List<Task<Data.Models.ChucNang.Games>>();
                    foreach (var node in textNodesCate)
                    {
                        //var urlDetailGame = node.SelectSingleNode(".//a[contains(@class,'tile')]").Attributes["href"].Value;
                        //var htmlDetailGameDoc = web.Load(urlDetailGame);
                        //Data.Models.ChucNang.Games game = SetDataGame(node, requestCrawl, htmlDetailGameDoc, stt);
                        //_repo.Add(game);
                        //stt++;
                        var ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//div[contains(@class,'tile-title')]").InnerText);
                        if (gamesFromDb.Where(x => x.ten == ten && x.category_id == requestCrawl.category_id).Count() == 0)
                        {
                            Task<Data.Models.ChucNang.Games> response = CallOneBlockGamesAsync(node, requestCrawl.category_id, stt, web);
                            tasks.Add(response);
                            stt++;
                            total_game++;
                        }
                    }
                    var all_data = await Task.WhenAll(tasks);
                    if (all_data.Any())
                    {
                        total_error_game += all_data.Where(x => x.iframe == "" || x.iframe == null).Count();
                        _repo.AddRange(all_data.Where(x => x.iframe != "" && x.iframe != null));
                    }
                }

                if (totalPage > 0) 
                {
                    for (int i = 2; i <= totalPage; i++)
                    {
                        var htmlCategoriesPageDoc = web.Load($"{cate.url_crawl}?page={i}");
                        var textNodesCatePage = htmlCategoriesPageDoc.DocumentNode.SelectNodes("//ul[@class='grid-row']/li");
                        if (textNodesCatePage != null)
                        {
                            List<Task<Data.Models.ChucNang.Games>> tasks = new List<Task<Data.Models.ChucNang.Games>>();
                            foreach (var node in textNodesCatePage)
                            {
                                var ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//div[contains(@class,'tile-title')]").InnerText);
                                if (gamesFromDb.Where(x => x.ten == ten && x.category_id == requestCrawl.category_id).Count() == 0)
                                {
                                    Task<Data.Models.ChucNang.Games> response = CallOneBlockGamesAsync(node, requestCrawl.category_id, stt, web);
                                    tasks.Add(response);
                                    stt++;
                                    total_game++;
                                }
                            }
                            var all_data = await Task.WhenAll(tasks);
                            if (all_data.Any())
                            {
                                total_error_game += all_data.Where(x => x.iframe == "" || x.iframe == null).Count();
                                _repo.AddRange(all_data.Where(x => x.iframe != "" && x.iframe != null));
                            }
                        }
                    }
                }

                #region update tổng game thành công, thất bại của danh mục games đang crawl
                if(cateUpdateTotal != null)
                {
                    cateUpdateTotal.total_game = cateUpdateTotal.total_game + (total_game - total_error_game);
                    cateUpdateTotal.total_error_game = total_error_game;
                    cateUpdateTotal.is_crawl = true;
                    cateUpdateTotal.ngay_chinh_sua = DateTime.Now;
                    _dbContext.Categories.Update(cateUpdateTotal);
                }
                #endregion

                int countGame = _dbContext.SaveChanges();
                if (countGame > 0)
                {
                    return $"Thêm mới thành công {total_game - total_error_game} game";
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Data.Models.ChucNang.Games SetDataGame(HtmlNode node, RequestCrawlGamesDTO requestCrawl, HtmlDocument htmlDetailGameDoc, int stt)
        {
            try
            {
                var game = new Data.Models.ChucNang.Games();
                var iframeRegex = new Regex(@"<iframe.*?src=[""'](http[s]?:\/\/.*?)[\""'].*?>", RegexOptions.IgnoreCase);
                var match = iframeRegex.Match(htmlDetailGameDoc.Text);

                if (match.Success)
                {
                    var iframe = match.Groups[1].Value;
                    int index = iframe.IndexOf('?');

                    if (index != -1)
                        game.iframe = iframe.Substring(0, index);
                    else
                        game.iframe = iframe;
                }

                game.category_id = requestCrawl.category_id;
                game.su_dung = true;
                game.so_thu_tu = stt;
                game.so_lan_choi = 0;
                game.like = 0;
                game.dislike = 0;
                game.is_new = false;
                game.is_trending = false;
                game.ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//div[contains(@class,'tile-title')]").InnerText);
                game.img = node.SelectSingleNode(".//img[contains(@class,'lazy')]").Attributes["data-src"].Value;
                game.mo_ta = htmlDetailGameDoc.DocumentNode.SelectSingleNode(".//div[contains(@class,'description')]").OuterHtml;

                return game;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task<Data.Models.ChucNang.Games> CallOneBlockGamesAsync(HtmlNode node, Guid category_id, int stt, HtmlWeb web)
        {
            Data.Models.ChucNang.Games res = new Data.Models.ChucNang.Games();
            try
            {
                var urlDetailGame = node.SelectSingleNode(".//a[contains(@class,'tile')]").Attributes["href"].Value;

                var htmlDetailGameDoc = web.Load(urlDetailGame);
                var game = new Data.Models.ChucNang.Games();


                var iframeRegex = new Regex(@"<iframe.*?src=[""']((http[s]?:)?\/\/.*?)[\""'].*?>", RegexOptions.IgnoreCase);
                var match = iframeRegex.Match(htmlDetailGameDoc.Text);

                if (match.Success)
                {
                    var iframe = match.Groups[1].Value;
                    int index = iframe.IndexOf('?');

                    if (index != -1)
                        game.iframe = iframe.Substring(0, index);
                    else
                        game.iframe = iframe;
                }

                game.category_id = category_id;
                game.su_dung = true;
                game.so_thu_tu = stt;
                game.so_lan_choi = 0;
                game.like = 0;
                game.dislike = 0;
                game.is_new = false;
                game.is_trending = false;
                game.img = node.SelectSingleNode(".//img[contains(@class,'lazy')]").Attributes["data-src"].Value;
                game.ten = HtmlEntity.DeEntitize(node.SelectSingleNode(".//div[contains(@class,'tile-title')]").InnerText);
                game.mo_ta = htmlDetailGameDoc.DocumentNode.SelectSingleNode(".//div[contains(@class,'description')]") != null ? htmlDetailGameDoc.DocumentNode.SelectSingleNode(".//div[contains(@class,'description')]").OuterHtml : "";
                game.slug = AppConst.GetSlug(game.ten);

                return game;
            }
            catch (Exception ex)
            {
                return res;
            }
        }

        public List<GamesPublicDTO> GetGamesNew()
        {
            try
            {
                var res = new List<GamesPublicDTO>();

                var data = _repo.Where(x => x.is_new == true).Take(100).ToList();
                res = _mapper.Map<List<Data.Models.ChucNang.Games>, List<GamesPublicDTO>>(data);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GamesPublicDTO> GetGamesByCategories(string slug_game)
        {
            try
            {
                var res = new List<GamesPublicDTO>();

                var data = _repo.Where(x => x.categories.slug == slug_game).ToList();
                res = _mapper.Map<List<Data.Models.ChucNang.Games>, List<GamesPublicDTO>>(data);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GamesDTO GetDetail(string slug)
        {
            try
            {
                var res = new GamesDTO();

                var game = _repo.Where(x => x.slug == slug).FirstOrDefault();
                if(game != null)
                {
                    res = _mapper.Map<Data.Models.ChucNang.Games, GamesDTO>(game);
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
