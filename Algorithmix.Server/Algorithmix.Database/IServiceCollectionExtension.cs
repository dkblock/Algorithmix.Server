using Algorithmix.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Database
{
    public static class IServiceCollectionExtension
    {
        public static void ConfigureDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfig>();
            var connectionString = configuration.ConnectionString;

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
