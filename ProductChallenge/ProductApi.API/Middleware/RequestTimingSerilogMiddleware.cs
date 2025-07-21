using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProductApi.API.Middleware
{
    public class RequestTimingSerilogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _timingLogger;
        public RequestTimingSerilogMiddleware(RequestDelegate next, Serilog.ILogger timingLogger)
        {
            _next = next;
            _timingLogger = timingLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            _timingLogger.Information("{Method} {Path} responded with status {StatusCode} in {ElapsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
