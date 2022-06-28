using _0sechill.Config;
using _0sechill.Dto.MailDto;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace _0sechill.Services.Class
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailSettings;
        private readonly ILogger<MailService> logger;

        public MailService(
            IOptions<MailSettings> mailSettings,
            ILogger<MailService> logger)
        {
            this.mailSettings = mailSettings.Value;
            this.logger = logger;
        }
        public async Task SendMailAsync(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.ToEmail));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // use SmtpClient from MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // if mail send fail, the content will be saved in folder mailsave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                logger.LogInformation("Error in sending mail, archiving at - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("mail is sent to " + mailContent.ToEmail);
        }
    }
}
