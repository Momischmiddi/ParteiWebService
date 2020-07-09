using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ParteiWebService.StorageManagers;
using System;
using System.IO;

namespace ParteiWebService
{
    public class Program
    {
        public static string allFiles = "";

        public static void Main(string[] args)
        {
            foreach(var f in Directory.GetFiles(Directory.GetCurrentDirectory())) {
                allFiles += f + ", ";
            }

            allFiles += Environment.NewLine + Environment.NewLine;

            BlobManager.Setup();
            CosmosManager.Setup();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
