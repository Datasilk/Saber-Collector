﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saber.Vendor;

namespace Saber.Vendors.Collector
{
    public class Startup : IVendorStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfigurationRoot config)
        {
            ConfigurePlugin();
        }

        private void ConfigurePlugin()
        {
            var file = App.MapPath("/Vendors/Collector/config.json");
            if (!System.IO.File.Exists(file))
            {
                Console.WriteLine("You must copy, rename, then modify \"/Vendors/Collector/config.template.json\" to \"/Vendors/Collector/config.json\" and restart Saber to use Collector.");
                return;
            }
            var config = new ConfigurationBuilder()
                    .AddJsonFile(file).Build();

            var section = "";
            try
            {
                switch (App.Environment)
                {
                    case Environment.development:
                        section = "browser:path:development";
                        break;
                    case Environment.production:
                        section = "browser:path:production";
                        break;
                    case Environment.staging:
                        section = "browser:path:staging";
                        break;
                }
                Cache.Add("browserPath", config.GetSection(section).Value);
            }
            catch (Exception)
            {
                Console.WriteLine("configuration section " + section + " does not contain any values in /Vendors/Collector/config.json");
            }
        }
    }
}