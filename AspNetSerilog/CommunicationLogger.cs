using AspNetSerilog.Extensions;
using AspNetSerilog.Extractors;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System;
using System.Linq;
using System.Net;
using System.Web;

namespace AspNetSerilog
{
    /// <summary>
    /// Communication Logger implementation
    /// </summary>
    public class CommunicationLogger : ICommunicationLogger
    {
        /// <summary>
        /// Default Log Information Title
        /// </summary>
        public const string DefaultInformationTitle = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";

        /// <summary>
        /// Default Log Error Title
        /// </summary>
        public const string DefaultErrorTitle = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";

        /// <summary>
        ///  Serilog Configuration
        /// </summary>
        public SerilogConfiguration SerilogConfiguration { get; set; }

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        /// <param name="logger"></param>
        public CommunicationLogger(SerilogConfiguration configuration)
        {
            this.SetupCommunicationLogger(configuration);
        }

        /// <summary>
        /// Constructor using global logger definition
        /// </summary>
        public CommunicationLogger()
        {
            this.SetupCommunicationLogger(null);
        }

        /// <summary>
        /// Log context 
        /// </summary>
        /// <param name="context"></param>
        public void LogData(HttpContext context)
        {
            var routeDisabled = (this.SerilogConfiguration.IgnoredRoutes?.ToList()
                .Where(r => context.Request.Path.ToString().StartsWith(r)).Any() == true);

            if ((context?.Items == null || context.Items.TryGetValue(DisableLoggingExtension.ITEM_NAME, out object disableSerilog) == false)
                && routeDisabled == false)
            {
                this.LogData(context, null);
            }
        }

        /// <summary>
        /// Log context and exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public void LogData(HttpContext context, Exception exception)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var statusCode = context.GetStatusCode(exception);

            object controller = "Unknow";
            context.Items.TryGetValue("Controller", out controller);

            object action = "Unknow";
            context.Items.TryGetValue("Action", out action);

            string exceptionMessage = null;
            string exceptionStackTrace = null;

            if (exception != null)
            {
                exceptionMessage = HandleFieldSize(exception.Message, ExceptionMaxLenghtExtension.ErrorMessageLenght);
                exceptionStackTrace = HandleFieldSize(exception.StackTrace, ExceptionMaxLenghtExtension.ErrorExceptionLenght);
            }

            LogContext.PushProperty("RequestBody", context.GetRequestBody(this.SerilogConfiguration.BlacklistRequest));
            LogContext.PushProperty("Method", context.Request.Method);
            LogContext.PushProperty("Path", context.GetPath(this.SerilogConfiguration.HttpContextBlacklist));
            LogContext.PushProperty("Host", context.GetHost());
            LogContext.PushProperty("Port", context.GetPort());
            LogContext.PushProperty("Url", context.GetFullUrl(this.SerilogConfiguration.QueryStringBlacklist, this.SerilogConfiguration.HttpContextBlacklist));
            LogContext.PushProperty("QueryString", context.GetRawQueryString(this.SerilogConfiguration.QueryStringBlacklist));
            LogContext.PushProperty("Query", context.GetQueryString(this.SerilogConfiguration.QueryStringBlacklist));
            LogContext.PushProperty("RequestHeaders", context.GetRequestHeaders(this.SerilogConfiguration.HeaderBlacklist));
            LogContext.PushProperty("Ip", context.GetIp());
            LogContext.PushProperty("User", context.GetUser());
            LogContext.PushProperty("IsSuccessful", statusCode < 400);
            LogContext.PushProperty("StatusCode", statusCode);
            LogContext.PushProperty("StatusDescription", ((HttpStatusCode)statusCode).ToString());
            LogContext.PushProperty("StatusCodeFamily", context.GetStatusCodeFamily(exception));
            LogContext.PushProperty("ProtocolVersion", context.Request.Protocol);
            LogContext.PushProperty("Controller", controller?.ToString());
            LogContext.PushProperty("Operation", action?.ToString()); 
            LogContext.PushProperty("ErrorException", exceptionStackTrace);
            LogContext.PushProperty("ErrorMessage", exceptionMessage);
            LogContext.PushProperty("ResponseContent", context.GetResponseContent(this.SerilogConfiguration.BlacklistResponse));
            LogContext.PushProperty("ContentType", context.Response.ContentType);
            LogContext.PushProperty("ContentLength", context.GetResponseLength());
            LogContext.PushProperty("ResponseHeaders", context.GetResponseHeaders(this.SerilogConfiguration.HeaderBlacklist));
            LogContext.PushProperty("Version", this.SerilogConfiguration.Version);
            LogContext.PushProperty("ElapsedMilliseconds", context.GetExecutionTime(this.SerilogConfiguration.TimeElapsedProperty));
            LogContext.PushProperty("RequestKey", context.GetRequestKey(this.SerilogConfiguration.RequestKeyProperty));
            LogContext.PushProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            if (context.Items.ContainsKey(LogAdditionalInfo.LogAdditionalInfoItemKey))
            {
                var additionalInfo = (LogAdditionalInfo) context.Items[LogAdditionalInfo.LogAdditionalInfoItemKey];

                if (additionalInfo?.Data != null)
                {
                    foreach (var item in additionalInfo.Data)
                    {
                        LogContext.PushProperty(item.Key, item.Value);
                    }
                }
            }

            if (exception != null || statusCode >= 500)
            {
                var errorTitle = this.SerilogConfiguration.ErrorTitle ?? DefaultErrorTitle;
                this.SerilogConfiguration.Logger.Error(errorTitle);
            }
            else
            {
                var informationTitle = this.SerilogConfiguration.InformationTitle ?? DefaultInformationTitle;
                this.SerilogConfiguration.Logger.Information(informationTitle);
            }
        }

        /// <summary>
        /// Initialize instance
        /// </summary>
        /// <param name="configuration"></param>
        private void SetupCommunicationLogger(SerilogConfiguration configuration)
        {
            this.SerilogConfiguration =
                configuration ?? new SerilogConfiguration();

            this.SerilogConfiguration.Logger =
                configuration?.Logger ?? Log.Logger;
        }

        /// <summary>
        /// Handle field size
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxSize"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string HandleFieldSize(string value, int maxSize, bool required = false, string defaultValue = "????")
        {
            if (string.IsNullOrWhiteSpace(value) && !required)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                value = defaultValue;
            }

            if (value.Length > maxSize)
            {
                return value.Substring(0, maxSize);
            }

            return value;
        }
    }
}
