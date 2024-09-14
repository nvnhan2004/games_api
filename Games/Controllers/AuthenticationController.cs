using Games.Data.Models.QTHT;
using Games.Services.Models;
using Games.Services.Services.Authentication;
using Games.Services.Services.Authentication.DTO;
using Games.Services.Services.MailKit;
using Games.Services.Services.MailKit.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Games.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _user;

        public AuthenticationController(
            UserManager<NguoiDung> userManager,
            RoleManager<Roles> roleManager,
            IEmailService emailService,
            IConfiguration configuration,
            SignInManager<NguoiDung> signInManager,
            IAuthentication user)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _signInManager = signInManager;
            _user = user;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            var tokenResponse = await _user.CreateUserWithTokenAsync(registerUser);
            if (tokenResponse.IsSuccess)
            {
                if (registerUser.Roles.Any())
                    await _user.AssignRoleToUserAsync(registerUser.Roles, tokenResponse.Response.User);

                #region gửi mail xác nhận đăng ký tài khoản
                // Url.Action: tạo 1 url chứa tên controller, method; khi click vào url sẽ gọi đến phương thức ConfirmEmail trong controller Authentication
                // Request.Scheme: trả về giao thức http or https trong url (tùy giao thức của yêu cầu hiện tại)
                // ex: https://localhost:7186/api/Authentication/ConfirmEmail?token=CfDJ8J%2FwvzTt7AFFoWjR%2FndpmLUJfZOCmn0VAyu3jNl%2B2Fcykuc7TXMg7ar6qkyhQld5ktWy4bjkyc7APwQam8IWU2eRObdxCuI6303P6AwWs09upuTdfyPNPW%2FRxqKLbwVCIwnx9BVo8I%2BS3ReqAVRhTNYY95Kqj2kxvgaocqm1nhMhYRkFnwOaML1EIZI%2BPO3T%2BhwAuyol4gkSV%2BKIKxVT%2FzaxdTWmErvk9nYJsQYToBMI0Dn%2FbAe%2B8Zcioz0Ivvae1Q%3D%3D&email=nvnhan2004@gmail.com
                //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = tokenResponse.Response.Token, email = registerUser.Email }, Request.Scheme);
                //var message = new Message(new string[] { registerUser.Email }, "Xác nhận đăng ký tài khoản Hệ thống Quản lý Tài chính", confirmationLink);
                //_emailService.SendMail(message);
                return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = $"Tạo tài khoản thành công!", IsSuccess = true });
                #endregion

                return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = $"Đăng ký tài khoản thành công!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { IsSuccess = false, Message = tokenResponse.Message });
        }

        #region
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        //{
        //    var user = await _userManager.FindByNameAsync(loginModel.Username);
        //    //if (!await _userManager.IsEmailConfirmedAsync(user))
        //    //{
        //    //    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = $"Tài khoản chưa hoàn thành xác nhận đăng ký, vui lòng kiểm Email xác nhận đã được gửi đến bạn!" });
        //    //}
        //    //if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
        //    //{
        //    //    var loginOtpResponse = await _user.GetOtpByLoginAsync(loginModel);
        //    //    if (loginOtpResponse.Response != null)
        //    //    {
        //    //        if (user.TwoFactorEnabled)
        //    //        {
        //    //            var token = loginOtpResponse.Response.Token;
        //    //            var message = new Message(new string[] { user.Email! }, "Mã OTP đăng nhập hệ thống Quản lý Tài chính", token);
        //    //            _emailService.SendMail(message);

        //    //            return StatusCode(StatusCodes.Status200OK,
        //    //             new Response { IsSuccess = loginOtpResponse.IsSuccess, Status = "Success", Message = loginOtpResponse.Message });
        //    //        }

        //    //        if (user != null)
        //    //        {
        //    //            var serviceResponse = await _user.GetJwtTokenAsync(user);
        //    //            return Ok(serviceResponse);
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = $"Mật khẩu không đúng, vui lòng kiểm tra lại!" });


        //    //return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = $"Không tồn tại tài khoản trong hệ thống!" });

        //    if (user.TwoFactorEnabled)
        //    {
        //        await _signInManager.SignOutAsync();
        //        var b = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
        //        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

        //        var message = new Message(new string[] { user.Email! }, "OTP Confrimation", token);
        //        _emailService.SendMail(message);

        //        return StatusCode(StatusCodes.Status200OK,
        //         new Response { Status = "Success", Message = $"We have sent an OTP to your Email {user.Email}" });
        //    }
        //    if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
        //    {
        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        foreach (var role in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, role));
        //        }


        //        var jwtToken = GetToken(authClaims);

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
        //            expiration = jwtToken.ValidTo
        //        });
        //        //returning the token...

        //    }
        //    return Unauthorized();
        //}


        //[HttpPost("Login-2FA")]
        //public async Task<IActionResult> LoginWithOTP(string code, string username)
        //{
        //    var jwt = await _user.LoginUserWithJWTokenAsync(code, username);
        //    if (jwt.IsSuccess)
        //    {
        //        return Ok(jwt);
        //    }
        //    return StatusCode(StatusCodes.Status404NotFound,
        //        new Response { Status = "Success", Message = $"Invalid Code" });
        //}
        #endregion

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO loginModel)
        {
            var loginOtpResponse = await _user.GetOtpByLoginAsync(loginModel);
            if (loginOtpResponse.Response != null)
            {
                var user = loginOtpResponse.Response.User;
                if (user.TwoFactorEnabled)
                {
                    var token = loginOtpResponse.Response.Token;
                    var message = new MessageDTO(new string[] { user.Email! }, "Mã OTP đăng nhập", token);
                    _emailService.SendMail(message);

                    return StatusCode(StatusCodes.Status200OK,
                     new ResponseDTO { IsSuccess = loginOtpResponse.IsSuccess, Status = "Success", Message = $"Hệ thống đã gửi mã OTP tới Email {user.Email}, vui lòng giữ bí mật mã OTP này" });
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var serviceResponse = await _user.GetJwtTokenAsync(user);
                    return Ok(serviceResponse);

                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string username)
        {
            var jwt = await _user.LoginUserWithJWTokenAsync(code, username);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new ResponseDTO { Status = "Error", Message = $"Đã có lỗi xảy ra!" });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout thành công!");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new MessageDTO(new string[] { user.Email }, "Xác nhận quên mật khẩu", forgotPasswordLink);
                _emailService.SendMail(message);

                return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"Vui lòng xác nhận yêu cầu thay đổi mật khẩu đã được gửi tới Email {user.Email}" });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseDTO { Status = "Error", Message = $"Không tồn tại tài khoản trong hệ thống!" });
        }

        //[HttpGet("SendMail")]
        //public IActionResult TestEmail()
        //{
        //    var message = new Message(new string[] { "nvnhan2004@gmail.com" }, "Test", "<h1>Đây là email test</h1>");
        //    _emailService.SendMail(message);
        //    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Gửi mail thành công!" });
        //}

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = "Xác nhận Email thành công!" });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "Tài khoản không tồn tại!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }


        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDTO { Token = token, Email = email };

            return StatusCode(StatusCodes.Status200OK, model);
        }

        [HttpPost("ResetPassword"), AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"Mật khẩu đã được thay đổi thành công!" });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseDTO { Status = "Error", Message = $"Không tồn tại tài khoản trong hệ thống!" });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenAutoDTO tokens)
        {
            var jwt = await _user.RenewAccessTokenAsync(tokens);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new ResponseDTO { Status = "Success", Message = $"Invalid Code" });
        }
    }
}
