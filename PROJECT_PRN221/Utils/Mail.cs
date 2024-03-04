using System;
using System.Drawing;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PROJECT_PRN221.Utils
{
    public class Mail
    {
        public class MailSettings
        {
            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }

        }

        public interface IEmailSender
        {
            Task SendEmailAsync(string email, string subject, string htmlMessage);
        }

        public class SendMailService : IEmailSender
        {
            private readonly MailSettings mailSettings;

            private readonly ILogger<SendMailService> logger;

            public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
            {
                mailSettings = _mailSettings.Value;
                logger = _logger;
                logger.LogInformation("Create SendMailService");
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var message = new MimeMessage();
                message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
                message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;

                var builder = new BodyBuilder();

                string image = "";
                if (subject.Equals("OTP"))
                {
                    image = "https://sp-ao.shortpixel.ai/client/to_auto,q_glossy,ret_img,w_768,h_570/https://vbee.vn/blog/wp-content/uploads/2020/10/cong-nghe-xac-thuc-sms-otp-1-768x570.jpg";

                }
                if(subject.Equals("Contact"))
                {
                    image = "https://blog.vantagecircle.com/content/images/2023/05/trust-in-the-workplace.png";

                }
                if (subject.Equals("Register"))
                {
                    image = "https://www.learningwithbiz.com/wp-content/uploads/2020/03/power-of-teamwork.png";
                }

                builder.HtmlBody = $@"<!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>HighTech Store</title>
                </head>
                <body>
                    <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;"">
                        <h2>{subject}</h2>
                        <hr>
                        <p>{htmlMessage}</p>
                        <img src=""{image}"" alt=""Image"" style=""max-width: 100%;"">
                        <hr>
                        <p>Regards,<br>HighTech Store</p>
                    </div>
                </body>
                </html>";

                message.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                try
                {
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {
                    System.IO.Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await message.WriteToAsync(emailsavefile);

                    logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                    logger.LogError(ex.Message);
                }

                smtp.Disconnect(true);

                logger.LogInformation("send mail to: " + email);
            }
        }
    }
}
