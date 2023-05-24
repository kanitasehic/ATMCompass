using ATMCompass.Core.Configuration;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Services;
using ATMCompass.Insfrastructure.Data;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Services;
using ATMCompass.Insfrastructure.HttpCLients;
using ATMCompass.Insfrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ATMCompass.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static void ConfigureCorsPolicy(this IServiceCollection services, string AllowSpecificOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                                      policy =>
                                      {
                                          policy.AllowAnyOrigin()
                                          .AllowAnyMethod()
                                          .AllowAnyHeader();
                                      });
            });
        }

        public static void SetupAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<ATMCompassDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.AuthConfiguration.Audience,
                    ValidIssuer = appSettings.AuthConfiguration.Issuer,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.AuthConfiguration.IssuerSigningKey)),
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IATMRepository, ATMRepository>();
        }

        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IUserService, UserService>(s =>
            {
                var userManager = s.GetService<UserManager<IdentityUser>>();
                return new UserService(userManager, appSettings.AuthConfiguration);
            });
            services.AddScoped<IOverpassAPIClient, OverpassAPIClient>();
            services.AddScoped<IGeoCodeClient, GeoCodeClient>();
            services.AddScoped<IATMService, ATMService>();
        }
    }
}
