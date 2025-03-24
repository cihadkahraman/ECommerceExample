using Serilog.Context;

namespace NotificationService.Api.Middlewares
{
    public class LogEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public LogEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();

            LogContext.PushProperty("CorrelationId", correlationId);
            LogContext.PushProperty("RequestPath", context.Request.Path);
            LogContext.PushProperty("UserId", context.User?.Identity?.Name ?? "anonymous");

            context.Items["CorrelationId"] = correlationId;

            await _next(context);
        }
    }
}
