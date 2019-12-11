using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesService.ConfigureProps
{
    public class ServiceProps : BaseProps
    {
        public ServiceProps(string propsArgs) : base(propsArgs)
        {
        }

        public ServiceProps(BaseProps baseProperties) : base(baseProperties)
        {
        }

        public ServiceProps(IDictionary<string, string> properties) : base(properties)
        {
        }

        public int ScanInterval
        {
            get
            {
                int interval;
                if (!Properties.ContainsKey(PropsNames.ScanInterval) || !int.TryParse(Properties[PropsNames.ScanInterval], out interval))
                {
                    interval = 5 * 1000;
                }
                return interval;
            }
        }

    }
}
