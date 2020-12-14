using M365.RoadMapInfo.Import;
using M365.RoadMapInfo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace M365.RoadMapInfo
{
    public class Startup
    {
        private const string ConnectionStringName = "PrimaryDatabase";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringName));
            });
            services.AddTransient<RoadMapImporter>();
            services.Configure<ImportConfig>(Configuration.GetSection("Importer"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();
            
            app.UseResponseCompression();

            //app.UseHttpsRedirection();
           
            
            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}