using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesService.ConfigureProps
{
    [DataContract]
    public class ScanProps : BaseProps
    {
        public ScanProps(string propsArgs) : base(propsArgs)
        {
        }

        public ScanProps(BaseProps baseProperties) : base(baseProperties)
        {
        }

        public ScanProps(IDictionary<string, string> properties) : base(properties)
        {
        }

        public ServiceProps ServiceProperties => new ServiceProps(Properties);
        public string OutputLocation => Properties[PropsNames.OutputsDir];

        public string[] InputLocations => Properties[PropsNames.InputDir].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

    }
}
