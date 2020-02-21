using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocScanExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            if (File.Exists(".env"))
            {
                logger.LogInformation("using environment variables from .env file");
                DotNetEnv.Env.Load();
            }
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID")))
                logger.LogCritical("'YOTI_CLIENT_SDK_ID' environment variable not found. " +
                    "Either pass these in the .env file, or as a standard environment variable.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}