using Serilog.Context;

namespace OrderService.Api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationHeaderKey = "X-Correlation-ID";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var hasHeader = context.Request.Headers.TryGetValue(CorrelationHeaderKey, out var correlationId);

            var correlationIdValue = hasHeader && !string.IsNullOrWhiteSpace(correlationId)
                ? correlationId.ToString()
                : Guid.NewGuid().ToString();

            using (LogContext.PushProperty("CorrelationId", correlationIdValue))
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers[CorrelationHeaderKey] = correlationIdValue;
                    return Task.CompletedTask;
                });

                context.Items["CorrelationId"] = correlationIdValue;

                await _next(context);
            }
        }
    }
}
