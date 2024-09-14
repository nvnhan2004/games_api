using Games.Data.Models.QTHT;
using Games.Services.Models;
using Games.Services.Services.Authentication.DTO;
using Games.Services.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Games.Services.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public Authentication(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            RoleManager<Roles> roleManager,
            IConfiguration configuration,
            IUserService userService
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<ApiResponseDTO<List<string>>> AssignRoleToUserAsync(List<string> roles, NguoiDung user)
        {
            var assignedRole = new List<string>();
            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    if (!await _userManager.IsInRoleAsync(user, role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        assignedRole.Add(role);
                    }
                }
            }

            return new ApiResponseDTO<List<string>>
            {
                IsSuccess = true,
                Message = "Role đã được gán cho người dùng!",
                StatusCode = 200,
                Response = assignedRole
            };
        }

        public async Task<ApiResponseDTO<CreateUserResponseDTO>> CreateUserWithTokenAsync(RegisterUserDTO registerUser)
        {
            var userByEmailExist = await _userManager.FindByEmailAsync(registerUser.Email);
            var userByUsernameExist = await _userManager.FindByNameAsync(registerUser.UserName);
            if (userByUsernameExist != null)
                return new ApiResponseDTO<CreateUserResponseDTO>
                {
                    IsSuccess = false,
                    Message = "Tài khoản đã tồn tại!",
                    StatusCode = 403,
                };
            if (userByEmailExist != null)
                return new ApiResponseDTO<CreateUserResponseDTO>
                {
                    IsSuccess = false,
                    Message = "Email đã được đăng ký!",
                    StatusCode = 403,
                };

            // Add user in the db
            NguoiDung user = new NguoiDung
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                PhoneNumber = registerUser.PhoneNumber,
                //TwoFactorEnabled = true // xác minh 2 lần
            };


            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return new ApiResponseDTO<CreateUserResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Tạo token thành công!",
                    StatusCode = 200,
                    Response = new CreateUserResponseDTO
                    {
                        Token = token,
                        User = user,
                    }
                };
            }
            return new ApiResponseDTO<CreateUserResponseDTO>
            {
                IsSuccess = false,
                Message = "Tạo tài khoản thất bại!",
                StatusCode = 500,
            };
        }

        public async Task<ApiResponseDTO<LoginOtpResponseDTO>> GetOtpByLoginAsync(LoginModelDTO loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null)
            {
                await _signInManager.SignOutAsync(); // xóa bỏ toàn bộ các thông tin phiên login trước
                var b = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true); // login
                if (user.TwoFactorEnabled)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    return new ApiResponseDTO<LoginOtpResponseDTO>
                    {
                        Response = new LoginOtpResponseDTO()
                        {
                            User = user,
                            Token = token,
                            IsTwoFactorEnable = user.TwoFactorEnabled
                        },
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = $"Hệ thống đã gửi mã OTP đến Email {user.Email}, vui lòng giữ bí mật OTP này!"
                    };

                }
                else
                {
                    return new ApiResponseDTO<LoginOtpResponseDTO>
                    {
                        Response = new LoginOtpResponseDTO()
                        {
                            User = user,
                            Token = string.Empty,
                            IsTwoFactorEnable = user.TwoFactorEnabled
                        },
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = $"Đăng nhập xác thực 2 bước không được bật!"
                    };
                }
            }
            else
            {
                return new ApiResponseDTO<LoginOtpResponseDTO>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = $"Tài khoản không tồn tại!"
                };
            }
        }

        public async Task<ApiResponseDTO<LoginResponseDTO>> GetJwtTokenAsync(NguoiDung user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.super_admin != null ? user.super_admin.ToString() : ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            //var userRoles = await _userManager.GetRolesAsync(user);
            //foreach (var role in userRoles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var jwtToken = GetToken(authClaims); //access token
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
            /*
             * int.TryParse trả về bool
             * nếu = true thì kết quả sau khi convert được gán cho refreshTokenValidity
             * nếu = false => refreshTokenValidity = 0
             * _: bỏ qua giá trị trả về của int.TryParse, ở đây là true hoặc false
             */


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenValidity);

            await _userManager.UpdateAsync(user);

            var permission = _userService.GetPermissionUser(user.Id);


            return new ApiResponseDTO<LoginResponseDTO>
            {
                Response = new LoginResponseDTO()
                {
                    AccessToken = new TokenTypeDTO()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        ExpiryTokenDate = jwtToken.ValidTo
                    },
                    RefreshToken = new TokenTypeDTO()
                    {
                        Token = user.RefreshToken,
                        ExpiryTokenDate = (DateTime)user.RefreshTokenExpiry
                    },
                    ProfileUser = new ProfileUserDTO()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        UserName = user.UserName,
                        super_admin = user.super_admin
                    },
                    PermissionUser = permission
                },

                IsSuccess = true,
                StatusCode = 200,
                Message = $"Tạo token thành công"
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInHours"], out int tokenValidityInHours);
            var expirationTimeUtc = DateTime.UtcNow.AddHours(tokenValidityInHours);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUtc, localTimeZone);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ApiResponseDTO<LoginResponseDTO>> LoginUserWithJWTokenAsync(string otp, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            // Lưu ý: cần xác nhận Email đăng ký (EmailConfirm = true) trước mới login 2 bước được
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", otp, false, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    return await GetJwtTokenAsync(user);
                }
                return new ApiResponseDTO<LoginResponseDTO>()
                {

                    Response = new LoginResponseDTO()
                    {

                    },
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = $"Không tồn tại tài khoản trong hệ thống, vui lòng thực hiện lấy lại mã OTP!"
                };
            }
            return new ApiResponseDTO<LoginResponseDTO>()
            {

                Response = new LoginResponseDTO()
                {

                },
                IsSuccess = false,
                StatusCode = 400,
                Message = $"Đã xảy ra lỗi"
            };
        }

        public async Task<ApiResponseDTO<LoginResponseDTO>> RenewAccessTokenAsync(RefreshTokenAutoDTO tokens)
        {
            var refreshToken = tokens.RefreshToken;
            var principal = GetClaimsPrincipal(tokens.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            //if (refreshToken.Token != user.RefreshToken && refreshToken.ExpiryTokenDate <= DateTime.Now)
            //{
            //    return new ApiResponseDTO<LoginResponseDTO>
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        Message = $"Token invalid or expired"
            //    };
            //}
            var response = await GetJwtTokenAsync(user);
            return response;
        }

        /*
         * xác thực và lấy thông tin user từ token -> kiểm tra tính hợp lệ token và trích xuất thông tin user dưới dạng claim
         * TokenValidationParameters: đối tượng chứa các tham số để xác thực token
         * 
         */
        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,   // ko kiểm tra đối tượng Audience của token
                ValidateIssuer = false,     // ko kiểm tra đối tượng Issuer của token
                ValidateIssuerSigningKey = true,    // kiểm tra khóa
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])), // lệnh kiểm tra khóa
                ValidateLifetime = false    // ko kiểm tra thời gian tồn tại của token còn bao lâu
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            // thông tin user sau khi xác minh
            return principal;

        }
    }
}
