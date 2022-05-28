using Algorithmix.Common.Settings;
using Microsoft.Extensions.Configuration;

namespace Algorithmix.Configuration
{
    public class DevelopmentConfiguration : IConfig
    {
        private readonly IConfiguration _configuration;

        public DevelopmentConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection");
        public string ClientUrl => _configuration["ClientUrl"];

        public MailSettings MailSettings => new MailSettings
        {
            Host = _configuration["Mail:Host"],
            Port = int.Parse(_configuration["Mail:Port"]),
            Address = _configuration["Mail:Address"],
            Password = _configuration["Mail:Password"]
        };

        public IdentitySettings IdentitySettings => new IdentitySettings
        {
            Secret = _configuration["Identity:Secret"],
            AccessTokenLifetimeInMinutes = int.Parse(_configuration["Identity:AccessTokenLifetimeInMinutes"]),
            RefreshTokenLifetimeInDays = int.Parse(_configuration["Identity:RefreshTokenLifetimeInDays"])
        };
    }
}
