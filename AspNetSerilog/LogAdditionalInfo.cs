using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AspNetSerilog
{
    public class LogAdditionalInfo
    {
        public static string LogAdditionalInfoItemKey = "LogAdditionalInfo";

        public LogAdditionalInfo(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Items.Add(LogAdditionalInfo.LogAdditionalInfoItemKey, this);
        }

        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}
