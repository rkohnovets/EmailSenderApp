using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using EmailSenderApp.Configuration;

namespace EmailSenderApp.Services
{
    /// <summary>
    /// The service used to send an email message using SMTP.
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Send an email message (asynchronously)
        /// </summary>
        /// <param name="recipients">An enumeration of email addresses, to whom you send the email message</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="message">The body of the email message</param>
        /// <returns></returns>
        Task SendAsync(IEnumerable<string> recipients, string subject, string message);
    }
    public class EmailSender: IMailSender
    {
        private readonly MailSettings mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendAsync(IEnumerable<string> recipients, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.From));
            emailMessage.To.AddRange(recipients.Select(email => new MailboxAddress("", email)));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailSettings.Host, mailSettings.Port, mailSettings.UseSSL);
                // using Ethereal - a fake SMTP Provider
                await client.AuthenticateAsync(mailSettings.From, mailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
