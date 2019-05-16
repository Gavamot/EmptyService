using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoReg.Core;
using VideoReg.Core.Thrends;

namespace VideoReg.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly IHostingEnvironment env;
        ILoggerFactory loggerFactory;

        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.env = env;
            Configuration = configuration;
            this.loggerFactory = loggerFactory;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
          
            services.AddSingleton<IDateService, DateService>();
            services.AddSingleton<IAppLogger>(new AppLogger(loggerFactory.CreateLogger("Main")));
            services.AddSingleton<ICache, CacheManager>();
            services.AddSingleton<ICamerasRep, TestCameraRep>();
            services.AddSingleton<IImgRep, HttpImgRep>();
            services.AddSingleton<VideoRegUpdator>();
            services.AddSingleton<CameraUpdatetor>();
            services.AddSingleton<IVideoRegInfoRep, TestVideoRegInfo>();
            services.AddSingleton<ITrendsRep, IFileTrendsRep>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            provider.GetService<VideoRegUpdator>().Start(null);
           
            app.UseMvc();
        }
    }
}
