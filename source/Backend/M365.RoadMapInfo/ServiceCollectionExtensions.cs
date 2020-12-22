using System.Collections.Generic;
using M365.RoadMapInfo.Authentication;
using M365.RoadMapInfo.Import;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace M365.RoadMapInfo
{
    public static class ServiceCollectionExtensions
    {
        public static void AddImporter(this IServiceCollection services)
        {
            services.AddTransient<DataImporter>();
        }
        
        public static void AddUserService(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IUserService, UserService>();
            services.Configure<List<ConfigUser>>(config.GetSection("Users"));
        }
    }
}