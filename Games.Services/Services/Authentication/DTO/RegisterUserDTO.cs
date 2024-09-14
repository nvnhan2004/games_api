using System.ComponentModel.DataAnnotations;

namespace Games.Services.Services.Authentication.DTO
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
