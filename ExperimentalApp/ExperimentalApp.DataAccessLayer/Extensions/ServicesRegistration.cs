using ExperimentalApp.Core.Enums;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.DataAccessLayer.Interfaces;
using ExperimentalApp.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExperimentalApp.DataAccessLayer.Extensions
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
        public static void AddServicesDependenciesDAL(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DvdRentalContext"));
            dataSourceBuilder.MapEnum<MPAA_Rating>();
            var dataSource = dataSourceBuilder.Build();
            services.AddDbContext<DvdRentalContext>(options =>
                options.UseNpgsql(dataSource));

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IFilmsRepository, FilmsRepository>();
        }
    }
}
