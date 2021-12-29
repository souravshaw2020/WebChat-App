using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Backened.Models;
using Backened.Hubs;
using Owin;

namespace Backened
{
    public class Startup
    {
        readonly string allowSpecificOrigins = "_allowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<SignalRChatContext>();
            services.AddDbContext<SignalRChatContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    });
            });
            services.AddControllers();
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy(allowSpecificOrigins, options => {
                    options
                        .WithOrigins("http://localhost:52967")
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyHeader()
                        .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS")
                        .AllowCredentials();
                });
            });

            //JSON Serializer
            services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                 .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SignalRChatContext context)
        {
            context.Database.Migrate();
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("/index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseFileServer(enableDirectoryBrowsing: false);

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:52967")
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS")
                .AllowCredentials();
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(allowSpecificOrigins);
                endpoints.MapHub<ChatHub>("/chathub").RequireCors(allowSpecificOrigins);
            });
        }
    }
}
