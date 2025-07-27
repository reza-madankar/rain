using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using rain.Model;

namespace rain.Helper.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireUserIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("x-userId", out StringValues userId) || string.IsNullOrEmpty(userId))
            {
                var message = new Message
                {
                    IsSuccess = false,
                    Description = "Please provide a user ID in the x-userId header to continue."
                };
                context.Result = new BadRequestObjectResult(message);
                return;
            }

            context.HttpContext.Items["UserId"] = userId.ToString();
            base.OnActionExecuting(context);
        }
    }
}
