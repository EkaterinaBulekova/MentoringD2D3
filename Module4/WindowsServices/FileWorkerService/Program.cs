using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Log4NetLoggerService;
using Microsoft.Extensions.Configuration;
using CofigurationService;
using FileProcessService;
using System.IO;
using System.Diagnostics;

namespace FileWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseWindowsService()
                 .ConfigureAppConfiguration((hostContext, config) => 
                     config
                         .SetBasePath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                 )
                .ConfigureServices((hostContext, services) => services
                .AddOptions()
                .Configure<WorkerSettings>(hostContext.Configuration.GetSection("WorkerSettings"))
                .AddSingleton<IConfigurator, Configurator>()
                .AddSingleton<IFileProcessService, ProcessService<ToPdfProcessor>>()                
                .AddHostedService<Worker>()
                .AddLogging(_ => _.AddLog4Net(true)));
    } 
}
