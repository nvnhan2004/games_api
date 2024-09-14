using Games.Data.Models.QTHT;

namespace Games.Services.Services.Authentication.DTO
{
    public class CreateUserResponseDTO
    {
        public string Token { get; set; }
        public NguoiDung User { get; set; }
    }
}
