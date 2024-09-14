using Games.Data.Models.QTHT;
using Games.Services.Models;
using Games.Services.Services.Authentication.DTO;

namespace Games.Services.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ApiResponseDTO<CreateUserResponseDTO>> CreateUserWithTokenAsync(RegisterUserDTO registerUser);
        Task<ApiResponseDTO<List<string>>> AssignRoleToUserAsync(List<string> roles, NguoiDung user);
        Task<ApiResponseDTO<LoginOtpResponseDTO>> GetOtpByLoginAsync(LoginModelDTO loginModel);
        Task<ApiResponseDTO<LoginResponseDTO>> GetJwtTokenAsync(NguoiDung user);
        Task<ApiResponseDTO<LoginResponseDTO>> LoginUserWithJWTokenAsync(string otp, string userName);
        Task<ApiResponseDTO<LoginResponseDTO>> RenewAccessTokenAsync(RefreshTokenAutoDTO tokens);
    }
}
