using Algorithmix.Common.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class EmailManager
    {
        private readonly EmailSettings _settings;

        public EmailManager(IConfiguration configuration)
        {
            var emailSettingsSection = configuration.GetSection("Email");
            _settings = emailSettingsSection.Get<EmailSettings>();
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Algorithmix", _settings.Address));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, false);
            await client.AuthenticateAsync(_settings.Address, _settings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
