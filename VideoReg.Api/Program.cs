using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VideoReg.Api
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "VideoReg.Api";
            //CreateWebHostBuilder(args).Build().Run();
            WebHostBuilder webHostBuilder = new WebHostBuilder();
            if (string.IsNullOrEmpty(webHostBuilder.GetSetting(WebHostDefaults.ContentRootKey)))
            {
                webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            }
            if (args != null)
            {
                webHostBuilder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            webHostBuilder.UseKestrel(delegate (WebHostBuilderContext builderContext, KestrelServerOptions options)
            {
                options.Configure(builderContext.Configuration.GetSection("Kestrel"));
            }).ConfigureAppConfiguration(delegate (WebHostBuilderContext hostingContext, IConfigurationBuilder config)
            {
                IHostingEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile("appsettings." + hostingEnvironment.EnvironmentName + ".json", optional: true, reloadOnChange: true);
                if (hostingEnvironment.IsDevelopment())
                {
                    Assembly assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                    if (assembly != null)
                    {
                        config.AddUserSecrets(assembly, optional: true);
                    }
                }
                config.AddEnvironmentVariables();
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            }).ConfigureLogging(delegate (WebHostBuilderContext hostingContext, ILoggingBuilder logging)
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
            })
                .ConfigureServices(delegate (WebHostBuilderContext hostingContext, IServiceCollection services)
                {
                    services.PostConfigure(delegate (HostFilteringOptions options)
                    {
                        if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
                        {
                            string[] array = hostingContext.Configuration["AllowedHosts"]?.Split(new char[1]
                            {
                                ';'
                            }, StringSplitOptions.RemoveEmptyEntries);
                            options.AllowedHosts = ((array != null && array.Length != 0) ? array : new string[1]
                            {
                                "*"
                            });
                        }
                    });
                    services.AddSingleton((IOptionsChangeTokenSource<HostFilteringOptions>)new ConfigurationChangeTokenSource<HostFilteringOptions>(hostingContext.Configuration));
                })
                .UseDefaultServiceProvider(delegate (WebHostBuilderContext context, ServiceProviderOptions options)
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });

            var host = webHostBuilder
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
