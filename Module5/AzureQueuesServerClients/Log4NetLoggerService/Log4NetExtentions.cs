using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace Log4NetLoggerService
{
    public static class Log4netExtensions
    {
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder factory, bool skipDiagnosticLogs)
        {
            var currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var configFile = Path.Combine(currentDir, "log4net.config");

            factory.AddProvider(new Log4NetProvider(configFile, skipDiagnosticLogs));
            return factory;
        }
    }
}
