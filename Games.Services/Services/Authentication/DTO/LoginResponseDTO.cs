namespace Games.Services.Services.Authentication.DTO
{
    public class LoginResponseDTO
    {
        public TokenTypeDTO AccessToken { get; set; }
        public TokenTypeDTO RefreshToken { get; set; }
        public ProfileUserDTO ProfileUser { get; set; }
        public List<string> PermissionUser { get; set; }
    }

    public class ProfileUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? super_admin { get; set; }
    }
    public class RefreshTokenAutoDTO
    {
        public string AccessToken { get; set; }
        public TokenTypeDTO RefreshToken { get; set; }
    }
}
