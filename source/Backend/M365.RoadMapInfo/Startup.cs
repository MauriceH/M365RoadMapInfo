using System.Security.Claims;
using M365.RoadMapInfo.Authentication;
using M365.RoadMapInfo.Model;
using Microsoft.AspNetCore.Authentication;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddImporter();
            services.AddUserService(Configuration);
            
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
                options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName));
            });
            

            services.AddAuthentication("BasicAuthentication").
                AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                    ("BasicAuthentication", null);
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.CanImport, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Policies.ClaimRoleImporter);
                });
            });
            
        }

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

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}