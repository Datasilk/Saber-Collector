using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            Console.WriteLine("Configuring Collector...");

            var file = App.MapPath("/Vendors/Collector/config.json");
            if (!File.Exists(file))
            {
                Console.WriteLine("You must copy, rename, then modify \"/Vendors/Collector/config.template.json\" to \"/Vendors/Collector/config.json\" and restart Saber to use Collector.");
                return;
            }
            var configfile = new ConfigurationBuilder()
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
                Article.storagePath = configfile.GetSection(section).Value;
                section = "wwwroot:" + environment;
                Article.wwwrootPath = configfile.GetSection(section).Value;
                section = "browser:endpoint:" + environment;
                Article.browserEndpoint = configfile.GetSection(section).Value;
                section = "llm:privatekey";
                LLM.PrivateKey = configfile.GetSection(section).Value;

                //create folders if they don't exist
                try
                {
                    if (!Directory.Exists(Article.storagePath + "articles"))
                    {
                        Console.WriteLine("Created folder " + Article.storagePath + "articles");
                        Directory.CreateDirectory(Article.storagePath + "articles");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not create folder " + Article.storagePath + "articles");
                }
                try
                {
                    if (!Directory.Exists(Article.storagePath + "files"))
                    {
                        Console.WriteLine("Created folder " + Article.storagePath + "files");
                        Directory.CreateDirectory(Article.storagePath + "files");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not create folder " + Article.storagePath + "files");
                }

                //load domain-level minimum download intervals (in seconds)
                section = "domains:downloads:minIntervals";
                Domains.DownloadMinimumIntervals = configfile.GetValue<int>(section);
                section = "";

                //load common words
                Models.Article.Rules.commonWords = Query.CommonWords.GetList().ToArray();
                Console.WriteLine("Loaded " + Models.Article.Rules.commonWords.Length + " common words from the database");

                //load wildcard blacklist
                var wildcards = Query.Blacklists.Domains.Wildcards.GetList();
                foreach(var wildcard in wildcards)
                {
                    try
                    {
                        Blacklist.Wildcards.Add(
                            new Regex("^" + Regex.Escape(wildcard)
                                .Replace("\\*", ".*")
                                .Replace("\\?", ".") + "$")
                        );
                    }
                    catch (Exception) { }
                }
                Console.WriteLine("Loaded " + Blacklist.Wildcards.Count + " blacklist wildcards from the database");

                //load language detector
                Languages.Detector = new LanguageDetection.LanguageDetector();
                Languages.Detector.AddAllLanguages();
                Console.WriteLine("Loaded 53 languages into the language detector");
            }
            catch (Exception ex)
            {
                if(section != "")
                {
                    Console.WriteLine("configuration section " + section + " does not exist in /Vendors/Collector/config.json");
                }
                else
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }

            //handle files for Collector wwwroot content
            app.Use(async (context, next) => {
                if (context.Request.Path.Value.Contains("/collector/"))
                {
                    try
                    {
                        var path = context.Request.Path.Value.Split("/collector/")[1];
                        var bytes = File.ReadAllBytesAsync(Article.wwwrootPath + path).Result;
                        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    }
                }
                else
                {
                    await next(context);
                }
            });
        }
    }
}
