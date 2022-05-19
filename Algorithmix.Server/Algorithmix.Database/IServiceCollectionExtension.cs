using Algorithmix.Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Database
{
    public static class IServiceCollectionExtension
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettingsSection = configuration.GetSection("Database");
            var settings = databaseSettingsSection.Get<DatabaseSettings>();

            var server = settings.Server;
            var port = settings.Port;
            var database = settings.DatabaseName;
            var password = settings.Password;

            var connectionString = $"Server={server},{port};Initial Catalog={database};User ID=SA;Password={password}";

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            InitializeData(services);
        }

        private static void InitializeData(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<ApplicationContext>();

            DataInitializer.Initialize(context);
        }
    }
}
