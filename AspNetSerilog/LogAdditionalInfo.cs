using System.Collections.Generic;

namespace AspNetSerilog
{
    public class LogAdditionalInfo
    {
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}
