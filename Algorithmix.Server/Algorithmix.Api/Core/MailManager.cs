using Algorithmix.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class MailManager
    {
        private readonly string _emailHost;
        private readonly int _emailPort;
        private readonly string _emailAddress;
        private readonly string _emailPassword;

        public MailManager(IConfig configuration)
        {
            _emailHost = configuration.MailSettings.Host;
            _emailPort = configuration.MailSettings.Port;
            _emailAddress = configuration.MailSettings.Address;
            _emailPassword = configuration.MailSettings.Password;
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Algorithmix", _emailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailHost, _emailPort, false);
            await client.AuthenticateAsync(_emailAddress, _emailPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
