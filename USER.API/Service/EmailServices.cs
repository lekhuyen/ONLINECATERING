using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using USER.API.Models;

namespace USER.API.Service
{
    public class EmailServices
    {
        private readonly EmailSetting emailSettings;
        public EmailServices(IOptions<EmailSetting> _emailSettings)
        {
            emailSettings = _emailSettings.Value;
        }
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var fromAddress = new MailAddress(emailSettings.FromMail);
            var toAddress = new MailAddress(emailRequest.ToMail);
            var smtp = new SmtpClient
            {
                Host = emailSettings.Host,
                Port = emailSettings.Port,
                EnableSsl = emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,

                Credentials = new NetworkCredential(emailSettings.FromMail, emailSettings.Password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailRequest.Subject,
                Body = emailRequest.HtmlContent,
                IsBodyHtml = true
            };
            await smtp.SendMailAsync(message);
        }
    }
}
