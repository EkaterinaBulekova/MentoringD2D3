using System;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using ImgScanerService.Interfaces;
using ImgScanerService.Services;
using ImgScanerService.Utils;
using LoggingDynamicProxy;
using NLog;
using NLog.Config;
using NLog.Targets;
using UtilitiesService.Files;
using UtilitiesService.Interfaces;
using UtilitiesService.ConfigureProps;

namespace WindowsServiceScanner
{
    class Program
    {
        private static void Main(string[] args)
        {
            var logFactory = GetFactory(args, @"C:\service\scanner.log");
            var logger = LoggingService.Logger.Current;
            var props = BaseProps.GetProperties(args);
            var logProps = new LogProps(props);

            logger.SetActualLogger(logFactory.GetLogger("ScanService"), logProps.UseCodeRewritingLogs);


            logger.LogInfo("Main");
            foreach (var arg in args)
            {
                logger.LogInfo(arg);
            }

            var scanProps = new ScanProps(props);
            var container = GetContainer(scanProps, logProps);
            
            var service = new ScannerService(container);

            if (args.Length > 0 && args[0].Equals("console"))
            {
                service.StartScanning();

                Console.ReadKey();
                service.StopScanning();
            }
            else
            {
                try
                {
                    ScannerService.Run(service);
                }
                catch (Exception e)
                {
                    if (logger != null)
                    {
                        logger.LogError(e);
                    }
                }
            }
        }
        private static LogFactory GetFactory(string[] args, string defaultPath)
        {
            string logPath = LogProps.GetLogPath(args, defaultPath);
            var logConfig = new LoggingConfiguration();

            var target = new FileTarget()
            {
                Name = "Def",
                FileName = logPath,
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);
            var consoleTarget = new ConsoleTarget
            {
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}",
                Name = "console"
            };

            logConfig.AddTarget(consoleTarget);
            logConfig.AddRuleForAllLevels(consoleTarget);

            var logFactory = new LogFactory(logConfig);

            return logFactory;
        }

        private static IContainer GetContainer(ScanProps parameters, LogProps logBaseProperties)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new LoggerInterceptor(logBaseProperties.UseDynamicProxyLogs)).Named<IInterceptor>("log-interceptor");

            builder.RegisterType<ScannerWorker>().As<IScannerWorker>().EnableInterfaceInterceptors().InterceptedBy("log-interceptor");
            builder.RegisterType<FileScanner>().As<IFileScanner>().EnableInterfaceInterceptors().InterceptedBy("log-interceptor");
            builder.RegisterType<FileSystemHelper>().As<IFileSystemHelper>().EnableInterfaceInterceptors().InterceptedBy("log-interceptor");
            builder.RegisterType<PdfMergerFiles>().As<IToPdfFilesMerger>().EnableInterfaceInterceptors().InterceptedBy("log-interceptor");
            builder.RegisterType<OutFolderService>().As<IOutputService>().EnableInterfaceInterceptors().InterceptedBy("log-interceptor");

            builder.RegisterInstance(LoggingService.Logger.Current).As<LoggingService.ILogger>();
            builder.RegisterInstance(parameters).As<ScanProps>();
            builder.RegisterInstance(parameters.ServiceProperties).As<ServiceProps>();
            builder.RegisterInstance(logBaseProperties).As<LogProps>();

            var container = builder.Build();
            return container;
        }
    }
}
