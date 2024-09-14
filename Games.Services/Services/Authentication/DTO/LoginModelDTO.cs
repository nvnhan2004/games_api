using System.ComponentModel.DataAnnotations;

namespace Games.Services.Services.Authentication.DTO
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "Tài khoản không được để trống!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        public string? Password { get; set; }
    }
}
