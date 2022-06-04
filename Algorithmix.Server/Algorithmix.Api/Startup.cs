using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Configuration;
using Algorithmix.Database;
using Algorithmix.Identity.Extensions;
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
        public Startup() { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors();

            services.AddConfiguration();
            services.AddCommonServices();
            services.AddIdentityServices();
            services.AddManagers();
            services.AddMappers();
            services.AddRepositories();
            services.AddValidators();
            services.ConfigureDatabase();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfig configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins(configuration.ClientUrl);
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
