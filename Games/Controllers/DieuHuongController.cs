using Games.Services.Models;
using Games.Services.Services.DieuHuong.DTO;
using Games.Services.Services.DieuHuong;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Games.Api.Controllers
{
    /// <summary>
    /// QTHT - điều hướng
    /// </summary>
    [Route("api/qtht-dieu-huong")]
    [Authorize]
    [ApiController]
    public class DieuHuongController : ControllerBase
    {
        private readonly IDieuHuongService _service;
        //private readonly INhatKyHeThongService _nhatkyhethongservice;
        public DieuHuongController(IDieuHuongService service
            //INhatKyHeThongService nhatkyhethongservice
            )
        {
            _service = service;
            //_nhatkyhethongservice = nhatkyhethongservice;
        }

        #region GetAll 
        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <param name="page">default = 1, lựa chọn page hiển thị</param>
        /// <param name="page_size"> cấu hình số dòng trả ra trong 1 page </param>
        /// <param name="sort">  sort = { field:value }</param>
        /// <param name="filter"> filter = { field:value }</param>
        /// <param name="search">Từ khóa tìm kiếm </param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get(int page = 1, int page_size = 0, string sort = null, string filter = null, string? search = null)
        {
            try
            {
                var result = await _service.GetManyAsync<DieuHuongDTO>(page, page_size, sort, filter, search);
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
        /// Lấy thông tin 1 người dùng
        /// </summary>
        /// <param name="id">là id của bản ghi</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync<DieuHuongDTO>(id);
                result.tree_dieu_huong = _service.GetTreeDieuHuong();
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
        /// Thêm mới điều hướng
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DieuHuongDTO obj)
        {
            try
            {
                var result = await _service.CreateAsync<DieuHuongDTO>(obj);

                //try
                //{
                //    if (result != null)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_dieu_huong";
                //        nkht.ban_ghi_id = result.id;
                //        nkht.duong_dan = "/dieuhuong/form/" + result.id.ToString();
                //        nkht.hanh_dong = "218";

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
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu cập nhật</param>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public async Task<ActionResult> Update([FromBody] DieuHuongDTO obj, Guid id)
        {
            try
            {
                var result = await _service.UpdateAsync<DieuHuongDTO>(obj, id);

                //try
                //{
                //    if (result != null)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_dieu_huong";
                //        nkht.ban_ghi_id = result.id;
                //        nkht.duong_dan = "/dieuhuong/form/" + result.id.ToString();
                //        nkht.hanh_dong = "219";

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
        /// Xóa 1 người dùng
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = _service.DeleteByMucLuc(id);

                //try
                //{
                //    if (result == true)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_dieu_huong";
                //        nkht.ban_ghi_id = id;
                //        nkht.duong_dan = "/dieuhuong/form/" + id.ToString();
                //        nkht.hanh_dong = "220";

                //        _nhatkyhethongservice.CreateNhatKyHeThong(nkht);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}

                if (result)
                    return Ok(result);
                return Conflict();
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
        /// Xóa nhiều người dùng 
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

        #region GetTreeDieuHuong
        /// <summary>
        /// Lấy tree điều hướng
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree-dieu-huong")]
        public async Task<ActionResult> GetTreeDieuHuong()
        {
            try
            {
                var result = _service.GetTreeDieuHuong();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion

        #region GetTreeDieuHuongForm
        /// <summary>
        /// Lấy tree điều hướng
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree-dieu-huong-form")]
        public async Task<ActionResult> GetTreeDieuHuongForm()
        {
            try
            {
                var result = _service.GetTreeDieuHuongForm();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region GetFlatDieuHuong
        /// <summary>
        /// Lấy flat điều hướng
        /// </summary>
        /// <returns></returns>
        [HttpGet("flat-dieu-huong")]
        public async Task<ActionResult> GetFlatDieuHuong()
        {
            try
            {
                var result = _service.GetFlatDieuHuong();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }
        #endregion
        #region Update SttOrder
        [HttpGet]
        [Route("updateSttOrder")]
        public void UpdateSttOrder()
        {
            try
            {
                _service.UpdateSttOrder();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region GetSttTheoDieuDuongCapTren
        /// <summary>
        /// Lấy tree điều hướng
        /// </summary>
        /// <returns></returns>
        [HttpGet("getsttdieuhuongcaptren")]
        public async Task<ActionResult> GetStt(Guid? dieu_huong_cap_tren_id)
        {
            try
            {
                var result = _service.GetStt(dieu_huong_cap_tren_id);
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
