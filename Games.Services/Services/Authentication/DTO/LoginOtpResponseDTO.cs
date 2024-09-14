using Games.Data.Models.QTHT;

namespace Games.Services.Services.Authentication.DTO
{
    public class LoginOtpResponseDTO
    {
        public string Token { get; set; }
        public bool IsTwoFactorEnable { get; set; }
        public NguoiDung User { get; set; }
    }
}
