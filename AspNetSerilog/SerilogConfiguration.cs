using Serilog;
using System.Collections.Generic;

namespace AspNetSerilog
{
    public class SerilogConfiguration
    {
        public string[] BlacklistRequest { get; set; }

        public string[] BlacklistResponse { get; set; }

        public string[] HeaderBlacklist { get; set; }

        public string[] QueryStringBlacklist { get; set; }
        
        public string[] HttpContextBlacklist { get; set; }

        public string InformationTitle { get; set; }

        public string ErrorTitle { get; set; }

        public string RequestKeyProperty { get; set; }

        public string AccountIdProperty { get; set; }

        public string TimeElapsedProperty { get; set; }

        public string Version { get; set; }

        public List<string> IgnoredRoutes { get; set; }

        public ILogger Logger { get; set; }
    }
}
