using System;
using System.Threading.Tasks;
using M365.RoadMapInfo.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace M365.RoadMapInfo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //Console.WriteLine(BCrypt.Net.BCrypt.EnhancedHashPassword("password-to-hash"));
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
                await db.Database.MigrateAsync();
            }
            
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
    
    
}