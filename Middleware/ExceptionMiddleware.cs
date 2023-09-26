namespace BokhandelensRESTApi.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {

        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong {@Machine} {@TraceId}",
                  Environment.MachineName,
                  System.Diagnostics.Activity.Current?.Id);

                await Results.Problem(
                    title: "Something went wrong",
                    statusCode: StatusCodes.Status500InternalServerError,
                    extensions: new Dictionary<string, Object?>
                    {
                    { "traceId", System.Diagnostics.Activity.Current?.Id },
                    })
                    .ExecuteAsync(context);
            }
        }
    }
}
