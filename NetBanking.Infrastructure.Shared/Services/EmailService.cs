using Microsoft.Extensions.Options;
using NetBanking.Core.Application.Dtos.Email;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Domain.Settings;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace NetBanking.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings MailSettings { get; }

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            MailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var email = new MimeMessage();

                email.Sender = MailboxAddress.Parse(request.From ?? MailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                // create body
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                // send email
                using var smtp = new SmtpClient();
                // smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(MailSettings.SmtpHost, MailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(MailSettings.SmtpUser, MailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
