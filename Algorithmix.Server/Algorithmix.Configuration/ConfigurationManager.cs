using Algorithmix.Common.Settings;
using Microsoft.Extensions.Configuration;
using System;

namespace Algorithmix.Configuration
{
    public interface IConfig
    {
        string ConnectionString { get; }
        string ClientUrl { get; }

        MailSettings MailSettings { get; }
        IdentitySettings IdentitySettings { get; }
    }

    public class ConfigurationManager : IConfig
    {
        private readonly IConfig _configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                _configuration = new DevelopmentConfiguration(configuration);
            else
                _configuration = new ProductionConfiguration();
        }

        public string ConnectionString => _configuration.ConnectionString;
        public string ClientUrl => _configuration.ClientUrl;
        public MailSettings MailSettings => _configuration.MailSettings;
        public IdentitySettings IdentitySettings => _configuration.IdentitySettings;
    }
}
