using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.BusinessLogic.Mappers;
using ExperimentalApp.BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExperimentalApp.BusinessLogic.Extensions
{
    /// <summary>
    /// Represents a class with dependencies and additional adjustment for application. For instance configuration for data base.
    /// </summary>
    public static class ServicesRegistration
    {
        /// <summary>
        /// Represent static method for additing configurations
        /// </summary>
        /// <param name="services">Represent a collection of application services</param>
        /// <param name="configuration">Represent configuration options</param>
        public static void AddServicesDependenciesBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IFilmsService, FilmsService>();
        }
    }
}