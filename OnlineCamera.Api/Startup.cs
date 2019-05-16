using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineCamera.Core;
using OnlineCamera.Core.Service;
using OnlineCamera.Core.Service.Statistic;
using Serilog;

namespace OnlineCamera.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ReadConfig();
        }

        public IConfiguration Configuration { get; }
        public Config Config { get; set; }


        private void ReadConfig()
        {
            Config = new Config();
            Configuration.GetSection("Settings").Bind(Config);
        }

        
        private VideoRegReqvestSettings[] GetSettings(string fileName)
        {
            var ipRep = new IpFileRep(fileName);
            var videoRegs = ipRep.ReadAll();
            if (!videoRegs.Any())
                throw new Exception($"VideoRegs not found in the file {fileName}");
           return videoRegs;
        } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddSingleton<IVideoRegApi, Http1Api>();
            services.AddSingleton<IStatistRegistrator, StatistRegistratorToDb>();
            services.AddSingleton(Config);
            services.AddSingleton<IAppLogger>(new AppLogger());
            services.AddSingleton<ICache, CacheManager>();
            services.AddSingleton<IStatistRegistrator, StatistRegistratorToDb>();
            services.AddSingleton<IOnlineCameraService, OnlineCameraService>();
            services.AddSingleton<IDateService, DateService>();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            try
            {
                var videoRegs = GetSettings(Config.IpFile);
                provider.GetService<IOnlineCameraService>().AddVideoRegs(videoRegs);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
