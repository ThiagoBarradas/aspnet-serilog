using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSerilog.Extensions
{
    public static class DisableLoggingExtension
    {
        /// <summary>
        /// Context item name | DisableLogging
        /// </summary>
        internal const string ITEM_NAME = "DisableLogging";

        /// <summary>
        /// Disable logging for information
        /// Exception will be logged
        /// </summary>
        /// <param name="context"></param>
        public static void DisableLogging(this HttpContext context)
        {
            context?.Items.Add(ITEM_NAME, true);
        }

        /// <summary>
        /// Disable logging for information
        /// Exception will be logged
        /// </summary>
        /// <param name="controller"></param>
        public static void DisableLogging(this ControllerBase controller)
        {
            controller?.HttpContext?.Items?.Add(ITEM_NAME, true);
        }
    }
}
