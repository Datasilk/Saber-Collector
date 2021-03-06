using System;
using System.IO;
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
            if (!File.Exists(file))
            {
                Console.WriteLine("You must copy, rename, then modify \"/Vendors/Collector/config.template.json\" to \"/Vendors/Collector/config.json\" and restart Saber to use Collector.");
                return;
            }
            var config = new ConfigurationBuilder()
                    .AddJsonFile(file).Build();

            var environment = "";
            var section = "";
            try
            {
                switch (App.Environment)
                {
                    case Environment.development:
                        environment = "development";
                        break;
                    case Environment.production:
                        environment = "production";
                        break;
                    case Environment.staging:
                        environment = "staging";
                        break;
                }
                section = "storage:" + environment;
                Article.storagePath = config.GetSection(section).Value;
                section = "browser:endpoint:" + environment;
                Article.browserEndpoint = config.GetSection(section).Value;

                //create folders if they don't exist
                if (!Directory.Exists(Article.storagePath + "articles"))
                {
                    Directory.CreateDirectory(Article.storagePath + "articles");
                }
                if (!Directory.Exists(Article.storagePath + "files"))
                {
                    Directory.CreateDirectory(Article.storagePath + "files");
                }

                //load common words
                Models.Article.Rules.commonWords = Query.CommonWords.GetList().ToArray();
            }
            catch (Exception)
            {
                Console.WriteLine("configuration section " + section + " does not exist in /Vendors/Collector/config.json");
            }
        }
    }
}
