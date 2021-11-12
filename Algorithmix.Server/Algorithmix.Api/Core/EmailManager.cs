using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class EmailManager
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailManager(IConfiguration configuration)
        {
            _senderEmail = configuration["Email:Mail"];
            _senderPassword = configuration["Email:Password"];
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Algorithmix", _senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync(_senderEmail, _senderPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
