using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M365.RoadMapInfo.Import;
using M365.RoadMapInfo.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace M365.RoadMapInfo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var scope =  host.Services.CreateScope();
            var db =  scope.ServiceProvider.GetRequiredService<MainDbContext>();
            await db.Database.MigrateAsync();
            
            var importer = new RoadMapImporter(db);
            await importer.ImportAsync();
            
            scope.Dispose();
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}