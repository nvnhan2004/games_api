using Games.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Games.Services.Common
{
    public class CurrentUser
    {
        IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserDTO Get()
        {
            var currentUser = new CurrentUserDTO();
            var super_admin = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            var id = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();
            var username = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            var email = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            currentUser.id = Guid.Parse(id);
            currentUser.username = username;
            currentUser.email = email;
            currentUser.super_admin = super_admin == "" ? false : bool.Parse(super_admin);

            return currentUser;
        }
    }
}
