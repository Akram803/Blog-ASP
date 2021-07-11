using Blog.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Blog.Services.Email
{
    public class EmailService 
    {
        private readonly SmtpClient _client;
        private SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
            _client = new SmtpClient();
        }

        public async Task SendEmail(string mail, string subject, string message) 
        {
            // create
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.Username));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            await _client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
            await _client.AuthenticateAsync(_settings.Username, _settings.Password);
            await _client.SendAsync(email);
            await _client.DisconnectAsync(true);

        }
    }
}
