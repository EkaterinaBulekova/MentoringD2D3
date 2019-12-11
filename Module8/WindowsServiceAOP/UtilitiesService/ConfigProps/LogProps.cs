using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesService.ConfigureProps
{
    public class LogProps : BaseProps
    {

        public LogProps(string propsArgs) : base(propsArgs)
        {
        }

        public LogProps(BaseProps baseProperties) : base(baseProperties)
        {
        }

        public LogProps(IDictionary<string, string> properties) : base(properties)
        {
        }

        public string LogPath => Properties[PropsNames.LogPath];

        public bool UseCodeRewritingLogs => Properties.ContainsKey(PropsNames.CodeRewritingLogs) && Properties[PropsNames.CodeRewritingLogs] == "true";
        public bool UseDynamicProxyLogs => Properties.ContainsKey(PropsNames.DynamicProxyLogs) && Properties[PropsNames.DynamicProxyLogs] == "true";


        public static string GetLogPath(string[] args, string defaultPath)
        {
            var properties = BaseProps.GetProperties(args);
            if (properties == null)
            {
                return defaultPath;
            }
            var logProperties = new LogProps(properties);

            return logProperties.LogPath;
        }


    }
}
