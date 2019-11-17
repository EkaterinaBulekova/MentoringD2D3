using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CofigurationService;
using Log4NetLoggerService;
using FileQueueService;

namespace FileServerService
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
                .ConfigureServices((hostContext, services) =>
                    services
                        .AddOptions()
                        .Configure<MoverSettings>(hostContext.Configuration.GetSection("MoverSettings"))
                        .AddSingleton<IConfigurator, Configurator>()
                        .AddSingleton<IQueueService, FileQueueServer>()
                        .AddHostedService<Worker>()
                        .AddLogging(_ => _.AddLog4Net(true)));
    }
}
