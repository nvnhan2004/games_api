using Games.Services.Models;
using Games.Services.Services.Users.DTO;
using Games.Services.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Games.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        //private IMapper _mapper;

        public UsersController(IUserService userService)
        {
            _userService = userService;
            //_mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int page = 1, int page_size = 0, string sort = null, string filter = null, string? search = null)
        {
            try
            {
                var users = await _userService.GetManyAsync<UserDTO>(page, page_size, sort, filter, search);
                return Ok(users);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(UserDTO model)
        {
            _userService.Create(model);
            return Ok(new { message = "Thêm người dùng thành công!" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, UserDTO model)
        {
            var res = _userService.Update(id, model);
            return Ok(new { message = "Cập nhật người dùng thành công!", data = res });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _userService.Delete(id);
            return Ok(new { message = "Xóa người dùng thành công!" });
        }

        [HttpPost("deletes")]
        public IActionResult Delete(List<Guid> ids)
        {
            _userService.Deletes(ids);
            return Ok(new { message = "Xóa người dùng thành công!" });
        }

        #region GetPermissionUser
        /*
        /// <summary>
        /// Lấy quyền người dùng
        /// </summary>
        /// <returns></returns>*/
        [HttpGet]
        [Route("get-permission-user")]
        public async Task<ActionResult> GetPermissionUser()
        {
            try
            {
                var id = User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
                Guid guid_id = Guid.Parse(id);
                var result = _userService.GetPermissionUser(guid_id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = ErrorCtr.ExtractErrorInfo(ex);
                return StatusCode(Convert.ToInt32(err.statusCode), err);
            }

        }
        #endregion
        #region
        [HttpGet]
        [Route("get-all-nguoi-dung")]
        public ActionResult GetAllNguoiDung()
        {
            try
            {
                var result = _userService.GetAllNguoiDung();
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
