using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Database;
using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Repository;
using Algorithmix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Algorithmix.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors();

            services.AddCommonServices();
            services.AddIdentityServices(Configuration);
            services.AddManagers();
            services.AddMappers();
            services.AddRepositories();
            services.AddValidators();
            services.ConfigureDatabase(Configuration).Wait();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var siteUrl = Configuration.GetValue<string>("SiteURL");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins(siteUrl);
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
