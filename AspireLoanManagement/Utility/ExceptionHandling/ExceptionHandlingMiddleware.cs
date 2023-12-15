using AspireLoanManagement.Utility.Logger;

namespace AspireLoanManagement.Utility.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAspireLogger _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IAspireLogger logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                // _logger.Log(LogLevel.Error, ex.StackTrace);
                throw;
            }
        }
    }
}
