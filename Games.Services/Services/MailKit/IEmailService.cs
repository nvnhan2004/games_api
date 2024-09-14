using Games.Services.Services.MailKit.DTO;

namespace Games.Services.Services.MailKit
{
    public interface IEmailService
    {
        void SendMail(MessageDTO message);
    }
}
