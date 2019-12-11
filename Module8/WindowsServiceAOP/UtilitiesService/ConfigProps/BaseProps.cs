using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UtilitiesService.ConfigureProps
{
    [DataContract]
    public class BaseProps
    {
        public BaseProps()
        {
        }

        public BaseProps(string propsArgs)
        {
            //-props:inputDirs=C:\ScanService\inputs\1;C:\ScanService\inputs\2|scanInterval=7000|outputsDir=C:\ScanService\out\|logPath=C:\ScanService\scannerservice.log|CodeRewritingLogs=false|DynamicProxyLogs=true
            PropsArgs = propsArgs;
            ParseProps(propsArgs);
        }

        public BaseProps(BaseProps baseProperties)
        {
            Properties = baseProperties.Properties;
            PropsArgs = baseProperties.PropsArgs;
        }

        public BaseProps(IDictionary<string, string> properties)
        {
            Properties = properties;
            PropsArgs = GetArgumentString(properties);
        }

        [DataMember]
        public IDictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();
        [DataMember]
        public string PropsArgs { get; private set; }

        public void Update(IDictionary<string, string> newVals)
        {
            if (newVals == null)
            {
                throw new ArgumentException("newVals");
            }
            Properties = Properties ?? new Dictionary<string, string>();
            foreach (var newValue in newVals)
            {
                if (Properties.ContainsKey(newValue.Key))
                {
                    Properties[newValue.Key] = newValue.Value;
                }
                else
                {
                    Properties.Add(newValue);
                }
            }
        }
        public void Update(BaseProps baseProps)
        {
            if (baseProps == null)
            {
                throw new ArgumentException("baseProps");
            }
            Update(baseProps.Properties);
        }

        protected void ParseProps(string propsArgs)
        {
            var props = PropsArgs.Split('|');
            foreach (var prop in props)
            {
                var splited = prop.Split('=');

                if (splited.Length >= 2)
                {
                    Properties.Add(splited[0], splited[1]);
                }
            }
        }

        public static BaseProps GetProperties(string[] args)
        {
            string propArgs = args.FirstOrDefault(x => x.StartsWith("-props:"));
            if (propArgs == null)
            {
                return null;
            }
            propArgs = new string(propArgs.Skip("-props:".Length).ToArray());
            var props = new BaseProps(propArgs);

            return props;
        }


        private string GetArgumentString(IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                return "";

            }
            return string.Join("|", properties.Select(x => $"{x.Key}={x.Value}"));
        }

        public override string ToString()
        {
            return GetArgumentString(this.Properties);

        }
    }
}
