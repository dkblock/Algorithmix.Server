using Algorithmix.Database;
using Algorithmix.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Algorithmix.Identity
{
    public static class IServiceCollectionExtension
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            var identitySettingsSection = configuration.GetSection("Identity");
            services.Configure<IdentitySettings>(identitySettingsSection);

            var identitySettings = identitySettingsSection.Get<IdentitySettings>();
            var secret = Encoding.ASCII.GetBytes(identitySettings.Secret);

            services.AddIdentity<ApplicationUserEntity, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<AuthenticationService, AuthenticationService>();
        }
    }
}
