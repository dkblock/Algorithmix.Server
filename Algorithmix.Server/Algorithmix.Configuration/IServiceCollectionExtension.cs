using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static void AddConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IConfig, ConfigurationManager>();
        }
    }
}
