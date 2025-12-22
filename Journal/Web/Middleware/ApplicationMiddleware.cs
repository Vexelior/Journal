namespace Web.Middleware;

public static class ApplicationMiddleware
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("RequestLoggingMiddleware");
            var request = context.Request;
            var method = request.Method;
            var path = request.Path;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            var userAgent = request.Headers["User-Agent"].ToString();
            logger.LogInformation("Incoming Request: {Method} {Path} from {IPAddress} - User Agent: {UserAgent}",
                method, path, ipAddress, userAgent);
            await next.Invoke();
        });
        return app;
    }
}
