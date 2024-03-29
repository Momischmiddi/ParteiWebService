using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ParteiWebService.StorageManagers;
using System;
using System.IO;

namespace ParteiWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
