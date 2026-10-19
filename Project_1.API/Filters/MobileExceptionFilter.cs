using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Project_1.API.Filters
{
    public class MobileExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode;
            string message;

            if (context.Exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = context.Exception.Message;
            }
            else if (context.Exception is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = context.Exception.Message;
            }
            else if (context.Exception is InvalidOperationException)
            {
                statusCode = StatusCodes.Status409Conflict;
                message = context.Exception.Message;
            }
            else if (context.Exception is DbUpdateConcurrencyException)
            {
                statusCode = StatusCodes.Status409Conflict;
                message = "Data changed. Reload before trying again.";
            }
            else if (context.Exception is DbUpdateException)
            {
                statusCode = StatusCodes.Status409Conflict;
                message = "The operation conflicts with existing data. Reload and try again.";
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Unable to complete the request.";

                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<MobileExceptionFilter>>();

                logger.LogError(context.Exception, "Mobile API request failed");
            }

            context.Result = new ObjectResult(new { message = message })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
