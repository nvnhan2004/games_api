using Games.Services.Models;
using Games.Services.Services.Games;
using Games.Services.Services.Games.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Games.Api.Controllers
{
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _service;
        //private readonly INhatKyHeThongService _nhatkyhethongservice;
        public GamesController(IGamesService service
            //INhatKyHeThongService nhatkyhethongservice
            )
        {
            _service = service;
            //_nhatkyhethongservice = nhatkyhethongservice;
        }

        #region GetAll 
        /// <summary>
        /// Lấy danh sách games
        /// </summary>
        /// <param name="page">default = 1, lựa chọn page hiển thị</param>
        /// <param name="page_size"> cấu hình số dòng trả ra trong 1 page </param>
        /// <param name="sort">  sort = { field:value }</param>
        /// <param name="filter"> filter = { field:value }</param>
        /// <param name="search">Từ khóa tìm kiếm </param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null)
        {
            try
            {
                var result = await _service.GetManyAsync<GamesDTO>(page, page_size, sort, filter, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region GetbyId
        /// <summary>
        /// Lấy thông tin 1 games
        /// </summary>
        /// <param name="id">là id của bản ghi</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync<GamesDTO>(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region Create
        /// <summary>
        /// Thêm mới games
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GamesDTO obj)
        {
            try
            {
                var result = await _service.CreateAsync<GamesDTO>(obj);

                //try
                //{
                //    if (result != null)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_quyen_nguoi_dung";
                //        nkht.ban_ghi_id = result.id;
                //        nkht.duong_dan = "/quyen-nguoidung/form/" + result.id.ToString();
                //        nkht.hanh_dong = "214";

                //        _nhatkyhethongservice.CreateNhatKyHeThong(nkht);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}

                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region Update
        /// <summary>
        /// Cập nhật games
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu cập nhật</param>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public async Task<ActionResult> Update([FromBody] GamesDTO obj, Guid id)
        {
            try
            {
                var result = await _service.UpdateAsync<GamesDTO>(obj, id);

                //try
                //{
                //    if (result != null)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_quyen_nguoi_dung";
                //        nkht.ban_ghi_id = result.id;
                //        nkht.duong_dan = "/quyen-nguoidung/form/" + result.id.ToString();
                //        nkht.hanh_dong = "215";

                //        _nhatkyhethongservice.CreateNhatKyHeThong(nkht);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}

                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region Delete
        /// <summary>
        /// Xóa 1 games
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                //try
                //{
                //    if (result > 0)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_quyen_nguoi_dung";
                //        nkht.ban_ghi_id = id;
                //        nkht.duong_dan = "";
                //        nkht.hanh_dong = "216";

                //        _nhatkyhethongservice.CreateNhatKyHeThong(nkht);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}

                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }

        }
        #endregion
        #region Deletes
        /// <summary>
        /// Xóa nhiều games 
        /// </summary>
        /// <param name="ids"> VD: ids=id1,id2,id3</param>
        /// <returns></returns>
        [HttpPost]
        [Route("deletes")]
        public async Task<ActionResult> Deletes([FromBody] List<Guid> ids)
        {

            try
            {
                var result = await _service.DeletesAsync(ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion

        #region Crawl games
        /// <summary>
        /// Crawl games
        /// </summary>
        /// <returns></returns>
        [HttpPost("crawl-games")]
        public async Task<ActionResult> CrawlGames([FromBody] RequestCrawlGamesDTO requestCrawl)
        {
            try
            {
                var result = await _service.CrawlGames(requestCrawl);
                return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = $"{result}", IsSuccess = true });
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion

        #region GetGamesNew
        /// <summary>
        /// GetGamesNew
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-games-new")]
        public async Task<ActionResult> GetGamesNew()
        {
            try
            {
                var result = _service.GetGamesNew();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion

        #region GetGamesByCategories
        /// <summary>
        /// GetGamesByCategories
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-by-categories/{cate}")]
        public async Task<ActionResult> GetGamesByCategories(string cate)
        {
            try
            {
                var result = _service.GetGamesByCategories(cate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion

        #region GetDetailCategories
        /// <summary>
        /// GetDetailCategories
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-detail/{slug}")]
        public async Task<ActionResult> GetDetail(string slug)
        {
            try
            {
                var result = _service.GetDetail(slug);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
    }
}
