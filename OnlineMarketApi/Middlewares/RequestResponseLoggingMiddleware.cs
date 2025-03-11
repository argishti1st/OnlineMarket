using System.Text.RegularExpressions;
using System.Text;
using Serilog;

namespace OnlineMarket.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isRequestResponseLoggingEnabled;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _isRequestResponseLoggingEnabled = config.GetValue<bool>("RequestResponseLoggingEnabled");
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Middleware is enabled only when the EnableRequestResponseLogging config value is set.  
            if (_isRequestResponseLoggingEnabled)
            {
                Log.Information($"HTTP request information:\n" +
                    $"\tMethod: {httpContext.Request.Method}\n" +
                    $"\tPath: {httpContext.Request.Path}\n" +
                    $"\tQueryString: {httpContext.Request.QueryString}\n" +
                    $"\tSchema: {httpContext.Request.Scheme}\n" +
                    $"\tHost: {httpContext.Request.Host}\n" +
                    $"\tBody: {MaskSensitiveData(await ReadBodyFromRequest(httpContext.Request))}");

                // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.  
                var originalResponseBody = httpContext.Response.Body;
                using var newResponseBody = new MemoryStream();
                httpContext.Response.Body = newResponseBody;

                // Call the next middleware in the pipeline  
                await _next(httpContext);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                Log.Information($"HTTP response information:\n" +
                    $"\tStatusCode: {httpContext.Response.StatusCode}\n" +
                    $"\tContentType: {httpContext.Response.ContentType}\n" +
                    $"\tBody: {MaskSensitiveData(responseBodyText)}");

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalResponseBody);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).  
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.  
            request.Body.Position = 0;
            return requestBody;
        }

        private string MaskSensitiveData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return jsonData;

            // Mask emails
            jsonData = Regex.Replace(jsonData, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", "******");

            // Mask userIds (Assuming userId is a numeric or UUID format)
            jsonData = Regex.Replace(jsonData, @"""userId""\s*:\s*""?[\w-]+""?", @"""userId"": ""******""");

            // Mask passwords
            jsonData = Regex.Replace(jsonData, @"""password""\s*:\s*"".*?""", @"""password"": ""******""");

            // mask jwt tokens
            jsonData = Regex.Replace(jsonData, @"""token""\s*:\s*"".*?""", @"""token"": ""******""");

            return jsonData;
        }
    }
}
