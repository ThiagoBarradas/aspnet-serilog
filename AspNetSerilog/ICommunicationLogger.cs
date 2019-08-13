using Microsoft.AspNetCore.Http;
using System;

namespace AspNetSerilog
{
    public interface ICommunicationLogger
    {
        SerilogConfiguration SerilogConfiguration { get; set; }

        void LogData(HttpContext context);

        void LogData(HttpContext context, Exception exception);
    }
}
