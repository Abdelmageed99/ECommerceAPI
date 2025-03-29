using System.Net;

namespace ECommerceAPI.Shared.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }

        }
    }
}
