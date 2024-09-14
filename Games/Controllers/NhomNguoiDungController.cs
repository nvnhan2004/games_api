using Games.Services.Models;
using Games.Services.Services.NhomNguoiDung.DTO;
using Games.Services.Services.NhomNguoiDung;
using Microsoft.AspNetCore.Mvc;

namespace Games.Api.Controllers
{
    [Route("api/qtht-nhom-nguoi-dung")]
    public class NhomNguoiDungController : ControllerBase
    {
        private readonly INhomNguoiDungService _service;
        //private readonly INhatKyHeThongService _nhatkyhethongservice;
        public NhomNguoiDungController(INhomNguoiDungService service
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
        public async Task<ActionResult> Get(int page = 1, int page_size = 0, string sort = null, string filter = null, string search = null)
        {
            try
            {
                var result = await _service.GetManyAsync<NhomNguoiDungDTO>(page, page_size, sort, filter, search);
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
                var result = await _service.GetByIdAsync<NhomNguoiDungDTO>(id);
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
        /// Thêm mới người dùng
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] NhomNguoiDungDTO obj)
        {
            try
            {
                var result = await _service.CreateAsync<NhomNguoiDungDTO>(obj);

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
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu cập nhật</param>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public async Task<ActionResult> Update([FromBody] NhomNguoiDungDTO obj, Guid id)
        {
            try
            {
                var result = await _service.UpdateAsync<NhomNguoiDungDTO>(obj, id);

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
        #region Thêm mới người dùng vào bảng trung gian
        /// <summary>
        /// Thêm mới người dùng vào bảng trung gian
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-user-to-group")]
        public async Task<ActionResult> AddUserToGroup([FromBody] UpdateNguoiDungFromNhomND obj)
        {
            try
            {
                var result = _service.AddUserToGroup(obj);

                //try
                //{
                //    if (result == true)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_quyen_nguoi_dung";
                //        nkht.ban_ghi_id = obj.nhom_id;
                //        nkht.duong_dan = "/quyen-nguoidung/form/" + obj.nhom_id.ToString();
                //        nkht.hanh_dong = "217";

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

        #region Thêm mới điều hướng vào bảng trung gian
        /// <summary>
        /// Thêm mới người dùng vào bảng trung gian
        /// </summary>
        /// <param name="obj">object dạng json chứa dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-permission")]
        public async Task<ActionResult> AddPermission([FromBody] AddPermission obj)
        {
            try
            {
                var result = _service.AddPermission(obj);

                //try
                //{
                //    if (result == true)
                //    {
                //        NhatKyHeThongDTO nkht = new NhatKyHeThongDTO();
                //        nkht.bang = "qtht_quyen_nguoi_dung";
                //        nkht.ban_ghi_id = obj.nhom_id;
                //        nkht.duong_dan = "/quyen-nguoidung/form/" + obj.nhom_id.ToString();
                //        nkht.hanh_dong = "217";

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
    }
}
