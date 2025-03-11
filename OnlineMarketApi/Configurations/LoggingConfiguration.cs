using Serilog;
using Elastic.Serilog.Sinks;
using Serilog.Extensions.Hosting;

namespace OnlineMarket.Api.Configurations
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return services;
        }
    }
}
