using System.Reflection;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.DataAccess.Database.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingTracker.DataAccess.Extension
{
    public static class ServiceCollectionExtender
    {
        public static void AddEntityFramework(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(ConnectionStrings.JoggingTrackerMain));

            services.AddDbContext<JoggingTrackerDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    config => config.MigrationsAssembly(typeof(JoggingTrackerDbContext).GetTypeInfo().Assembly.FullName));
            });
        }
    }
}
