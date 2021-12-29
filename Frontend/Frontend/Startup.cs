using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Frontend
{
    public class Startup
    {
        readonly string allowSpecificOrigins = "_allowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable Cors
            services.AddCors(c =>
            {
                c.AddPolicy(allowSpecificOrigins, options => options
                .WithOrigins("http://localhost:55474").SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS"));
            });

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

             app.UseStaticFiles();
             if (!env.IsDevelopment())
             {
                 app.UseSpaStaticFiles();
             }

             app.UseRouting();
            app.UseCors(builder => {
                builder.WithOrigins("http://localhost:55474")
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS");
            });
            app.UseAuthorization();

            /*app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller}/{action=Index}/{id?}");
             });*/

             app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
                 });

            
             app.UseSpa(spa =>
             {
                 // To learn more about options for serving an Angular SPA from ASP.NET Core,
                 // see https://go.microsoft.com/fwlink/?linkid=864501

                 spa.Options.SourcePath = "ClientApp";

                 if (env.IsDevelopment())
                 {
                     spa.UseAngularCliServer(npmScript: "start");
                 }
             });
        }
    }
}
