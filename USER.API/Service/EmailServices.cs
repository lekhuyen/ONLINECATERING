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

        public async Task SendEmailAsync(EmailBanned emailBanned)
        {
            var fromAddress = new MailAddress(emailSettings.FromMail);
            var toAddress = new MailAddress(emailBanned.ToMail);
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
                Subject = emailBanned.Subject,
                Body = emailBanned.HtmlContent,
                IsBodyHtml = true
            };
            await smtp.SendMailAsync(message);
        }

        public async Task SendBanNotificationEmail(string recipientEmail, string userName)
        {
            var emailBanned = new EmailBanned
            {
                ToMail = recipientEmail,
                Subject = "Account Banned Notification",
                HtmlContent = $"<p>Dear {userName},</p>" +
                              "<p>We regret to inform you that your account has been banned due to a violation of our policies.</p>" +
                              "<p>If you believe this action was taken in error, please contact our support team.</p>" +
                              "<p>Best regards,<br>OnlineCatering</p>"
            };

            await SendEmailAsync(emailBanned);
        }

        public async Task SendUnbanNotificationEmail(string recipientEmail, string userName)
        {
            var emailBanned = new EmailBanned
            {
                ToMail = recipientEmail,
                Subject = "Account Unbanned Notification",
                HtmlContent = $"<p>Dear {userName},</p>" +
                              "<p>We are pleased to inform you that your account has been reinstated. If you have any questions or need further assistance, please contact our support team.</p>" +
                              "<p>Best regards,<br>OnlineCatering</p>"
            };

            await SendEmailAsync(emailBanned);
        }
    }
}
