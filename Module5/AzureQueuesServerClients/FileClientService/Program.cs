using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CofigurationService;
using Log4NetLoggerService;
using FileQueueService;
using System.Configuration;
using System;

namespace FileServerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AddAppSettings<string>("MoverSettings:ClientName", Guid.NewGuid().ToString());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseWindowsService()
                 .ConfigureAppConfiguration((hostContext, config) =>
                     config
                         .SetBasePath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables())
                .ConfigureServices((hostContext, services) => services
                .AddOptions()
                .Configure<MoverSettings>(hostContext.Configuration.GetSection("MoverSettings"))
                .AddSingleton<IConfigurator, Configurator>()
                .AddSingleton<IQueueService, FileQueueClient>()
                .AddHostedService<Worker>()
                .AddLogging(_ => _.AddLog4Net(true)));

        public static void AddAppSettings<T>(string key, T value)
        {
            try
            {
                var filePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "appSettings.json");
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                var sectionPath = key.Split(":")[0];
                if (!string.IsNullOrEmpty(sectionPath))
                {
                    var keyPath = key.Split(":")[1];
                    var oldvalue = ((T)jsonObj[sectionPath][keyPath])?.ToString();
                    if (string.IsNullOrWhiteSpace(oldvalue))
                        jsonObj[sectionPath][keyPath] = value;
                }
                else
                {
                    jsonObj[sectionPath] = value;
                }
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);

            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
