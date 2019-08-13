using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetSerilog
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly ICommunicationLogger CommunicationLogger;

        public SerilogMiddleware(
            RequestDelegate next, 
            ICommunicationLogger communicationLogger)
        {
            this.Next = next;
            this.CommunicationLogger = communicationLogger;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalResponse = context.Response.Body;
            context.Response.Body = new MemoryStream();

            await this.Next(context);

            if (context.Items.ContainsKey("Exception"))
            {
                var exception = (Exception)context.Items["Exception"];
                this.CommunicationLogger.LogData(context, exception);
            }
            else
            {
                this.CommunicationLogger.LogData(context);
            }

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            context.Response.Body.CopyTo(originalResponse);
        }
    }


    public static class SerilogMiddlewareExtension
    {
        public static void UseAspNetSerilog(this IApplicationBuilder app)
        {
            app.UseMiddleware<SerilogMiddleware>();
        }
    }
}
