using System.ComponentModel.DataAnnotations;

namespace Games.Services.Services.Authentication.DTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không đúng")]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token không được để trống")]
        public string Token { get; set; }
    }
}
