namespace Games.Services.Services.MailKit.DTO
{
    public class EmailConfiguarationDTO
    {
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }

    }
}
