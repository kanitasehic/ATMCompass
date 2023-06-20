using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Services;
using ATMCompass.Insfrastructure.HttpCLients;
using ATMCompass.Insfrastructure.Repositories;

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

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IATMRepository, ATMRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IOverpassAPIClient, OverpassAPIClient>();
            services.AddScoped<IGeoCodeClient, GeoCodeClient>();
            services.AddScoped<IATMService, ATMService>();
            services.AddScoped<IGraphHopperClient, GraphHopperClient>();
        }
    }
}
