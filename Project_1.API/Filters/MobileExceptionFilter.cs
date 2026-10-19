using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
namespace Project_1.API.Filters
{
    public class MobileExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (status, message) = context.Exception switch
            {
                KeyNotFoundException ex => (404, ex.Message),
                ArgumentException ex => (400, ex.Message),
                InvalidOperationException ex => (409, ex.Message),
                DbUpdateConcurrencyException => (409, "Data changed. Reload before trying again."),
                DbUpdateException => (409, "The operation conflicts with existing data. Reload and try again."),
                _ => (500, "Unable to complete the request.")
            };
            if (status == 500)
                context.HttpContext.RequestServices.GetRequiredService<ILogger<MobileExceptionFilter>>()
                    .LogError(context.Exception, "Mobile API request failed");
            context.Result = new ObjectResult(new { message }) { StatusCode = status };
            context.ExceptionHandled = true;
        }
    }
}
