using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using VisualAlgorithms.Database;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Repository;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCommonServices();
            services.AddMappers();
            services.AddRepositories();
            await services.ConfigureDatabase(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
