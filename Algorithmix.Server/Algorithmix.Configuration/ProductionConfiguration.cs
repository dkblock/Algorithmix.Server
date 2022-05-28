using Algorithmix.Common.Settings;
using System;

namespace Algorithmix.Configuration
{
    public class ProductionConfiguration : IConfig
    {
        public string ConnectionString => GetValue("ConnectionStrings__Algorithmix");
        public string ClientUrl => GetValue("Client__Url");

        public MailSettings MailSettings => new MailSettings
        {
            Host = GetValue("Mail__Host"),
            Port = int.Parse(GetValue("Mail__Port")),
            Address = GetValue("Mail__Credentials__Login"),
            Password = GetValue("Mail__Credentials__Password")
        };

        public IdentitySettings IdentitySettings => new IdentitySettings
        {
            Secret = "ALGORITHMIX_SECRET_KEY",
            AccessTokenLifetimeInMinutes = 20,
            RefreshTokenLifetimeInDays = 30
        };

        private string GetValue(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
