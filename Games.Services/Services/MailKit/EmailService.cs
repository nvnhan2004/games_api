using Games.Services.Services.MailKit.DTO;
using MimeKit;
using MailKit.Net.Smtp;

namespace Games.Services.Services.MailKit
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguarationDTO _emailConfiguaration;

        public EmailService(EmailConfiguarationDTO emailConfiguaration) => _emailConfiguaration = emailConfiguaration;

        public void SendMail(MessageDTO message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MessageDTO message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Games", _emailConfiguaration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            //{
            //    Text = message.Content
            //};
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };
            //emailMessage.Body = new BodyBuilder()
            //{
            //    TextBody = message.Content
            //}.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguaration.SmtpServer, _emailConfiguaration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguaration.UserName, _emailConfiguaration.Password);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
