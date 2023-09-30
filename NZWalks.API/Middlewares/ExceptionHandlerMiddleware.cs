using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var erroId = Guid.NewGuid();

                // Log this exception
                _logger.LogError(ex, $"{erroId} : {ex.Message}");
                // Return a custom Error Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = erroId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                }; 

                await httpContext.Response.WriteAsJsonAsync(error);
                              
            }
}
    }
}
