using Serilog;
using System.Collections.Generic;

namespace AspNetSerilog
{
    public class SerilogConfiguration
    {
        public string[] Blacklist { get; set; }

        public string InformationTitle { get; set; }

        public string ErrorTitle { get; set; }

        public string RequestKeyProperty { get; set; }

        public string AccountIdProperty { get; set; }

        public string TimeElapsedProperty { get; set; }

        public IEnumerable<string> IgnoredRoutes { get; set; }

        public ILogger Logger { get; set; }
    }
}
