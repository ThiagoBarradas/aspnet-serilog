using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AspNetSerilog
{
    public class SerilogFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("Controller", context?.RouteData.Values["controller"]);
            context.HttpContext.Items.Add("Action", context?.RouteData.Values["action"]);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }
    }
}
