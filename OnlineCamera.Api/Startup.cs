﻿using System;
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
            var cacheManager = new CacheManager(new DateService(), Config);
            var statService = new StatistRegistratorToDb();

            IAppLogger log = new AppLogger();
            var cameraService = new OnlineCameraService(new Http1Api(), cacheManager, statService, Config, log);
            try
            {
                var videoRegs = GetSettings(Config.IpFile);
                cameraService.AddVideoRegs(videoRegs);
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                Environment.Exit(1);
            }

            services.AddSingleton<IAppLogger>(log);
            services.AddSingleton<ICache>(cacheManager);
            services.AddSingleton<IStatistRegistrator>(statService);
            services.AddSingleton<IOnlineCameraService>(cameraService);

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
